using Aukcionas.Auth.Model;
using Aukcionas.Data;
using Aukcionas.Models;
using Aukcionas.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Aukcionas.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ForumRestUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AdminController(UserManager<ForumRestUser> userManager, ILogger<AuthController> logger, DataContext dataContext, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpDelete("deleteuser/{username}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUserByUsername(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                if(username == "admin")
                {
                    return BadRequest("Cant delete this user");
                }

                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
        [HttpGet("usernames")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetAllUsernames()
        {
            try
            {
                var usernames = _dataContext.Users
            .Where(u => !_dataContext.UserRoles
                            .Any(ur => ur.UserId == u.Id &&
                                       _dataContext.Roles
                                              .Any(r => r.Id == ur.RoleId &&
                                                        r.Name == "Admin")))
            .Select(u => u.UserName)
            .ToList();
                return Ok(usernames);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching usernames: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching usernames");
            }
        }

        [HttpGet("auctions")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllAuctions()
        {
            try
            {
                var currentDate = DateTime.UtcNow;
                var oneMonthAgo = currentDate.AddMonths(-1);
                var oneMonthAhead = currentDate.AddMonths(1);

                var auctions = await _dataContext.Auctions
                    .Where(a => a.auction_end_time >= oneMonthAgo && a.auction_start_time <= oneMonthAhead)
                    .Select(a => new { Id = a.id, Name = a.name , a.auction_start_time, a.auction_end_time, a.category})
                    .ToListAsync();

                return Ok(auctions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching auctions: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching auctions");
            }
        }
        [HttpDelete("deleteauction/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                var auction = await _dataContext.Auctions.FindAsync(id);
                if (auction == null)
                {
                    return NotFound();
                }
                if(auction.auction_won== true && auction.is_Paid == true)
                {
                    return BadRequest("Cannot delete this auction, because it was won and paid for.");
                }

                _dataContext.Auctions.Remove(auction);
                await _dataContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting auction: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the auction.");
            }
        }
        [HttpPost("endauction/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EndAuction(int id)
        {
            try
            {
                var auction = await _dataContext.Auctions.FindAsync(id);
                if (auction == null)
                {
                    return NotFound();
                }
                if (auction.auction_won == true && auction.is_Paid == true && auction.auction_ended==true && auction.auction_end_time < DateTime.Now)
                {
                    return BadRequest("Cannot end this this auction, because it was won and paid for.");
                }
                auction.auction_end_time = DateTime.Now;
                auction.auction_ended = true;
                if (auction.bidding_amount_history.Any())
                {
                    double highestBid = auction.bidding_amount_history.Max();
                    if (highestBid >= auction.min_buy_price)
                    {
                        auction.auction_won = true;
                        var user = await _userManager.FindByNameAsync(auction.auction_biders_list.Last());
                        auction.auction_winner = user.UserName;
                        user.Auctions_Won.Add(auction.id);
                        if (user.CollectData == true)
                        {
                            var existingRecommendation = await _dataContext.Recommendations
                            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.AuctionId == auction.id && r.InteractionType == InteractionType.Win);

                            if (existingRecommendation == null)
                            {
                                var recommendation = new Recommendations
                                {
                                    UserId = user.Id,
                                    AuctionId = auction.id,
                                    CreatedAt = DateTime.Now,
                                    AuctionCategory = auction.category,
                                    InteractionType = InteractionType.Win
                                };
                                _dataContext.Recommendations.Add(recommendation);
                            }
                        }

                        var auctionOwner = await _userManager.FindByIdAsync(auction.username);

                        var payment = new PaymentUtils(_configuration);
                        var emailToken = payment.GeneratePaymentToken(user.Id, auction.id);
                        var paymentLink = new PaymentLinks
                        {
                            Payment_Link = $"http://localhost:4200/payment?token={emailToken}",
                            AuctionId = auction.id,
                            UserId = user.Id
                        };
                        _dataContext.PaymentLinks.Add(paymentLink);

                        string from = _configuration["EmailConfiguration:From"];
                        var emailModelBuyer = new EmailModel(user.Email, "Auction win", EmailBody.EmailPaymentForUser(emailToken, auction.name, auction.bidding_amount_history.Last()), "Auction winner confirmation");
                        _emailService.SendEmail(emailModelBuyer);

                        var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has a winner", EmailBody.EmailAuctionWonForOwner(auction.name, auction.bidding_amount_history.Last(), user.UserName, auction.id), "Auction ended");
                        _emailService.SendEmail(emailModelOwner);
                    }
                    else
                    {
                        var auctionOwner = await _userManager.FindByIdAsync(auction.username);
                        var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has ended", EmailBody.EmailAuctionEndedForOwner(auction.name, auction.id), "Auction ended");
                        _emailService.SendEmail(emailModelOwner);
                    }
                }
                else
                {
                    var auctionOwner = await _userManager.FindByIdAsync(auction.username);
                    var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has ended", EmailBody.EmailAuctionEndedForOwner(auction.name, auction.id), "Auction ended");
                    _emailService.SendEmail(emailModelOwner);
                }

                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ending auction: {ex.Message}");
                return StatusCode(500, "An error occurred while ending the auction.");
            }
        }
    }
}
