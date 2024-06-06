using Aukcionas.Data;
using Aukcionas.Models;
using Aukcionas.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aukcionas.Auth.Model;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Aukcionas.Utils;
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Aukcionas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ForumRestUser> _userManager;
        private readonly ILogger<AuctionController> _logger;
        private readonly IHubContext<BiddingHub> _hubContext;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //private readonly ICloudStorageService _cloudStorageService;
        public AuctionController(IWebHostEnvironment webHostEnvironment,UserManager<ForumRestUser> userManager,DataContext context, ILogger<AuctionController> logger, IConfiguration configuration, IHubContext<BiddingHub> hubContext /**ICloudStorageService cloudStorageService**/)
        {
            _userManager = userManager;
            _dataContext = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hubContext = hubContext;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            //_cloudStorageService = cloudStorageService;
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            try
            {
                var dbAuction = await _dataContext.Auctions.FindAsync(auction.id);
                if (dbAuction == null)
                    return BadRequest("Auction not found");
                dbAuction.name = auction.name;
                dbAuction.country = auction.country;
                dbAuction.city = auction.city;
                dbAuction.bid_ammount = auction.bid_ammount;
                dbAuction.min_buy_price = auction.min_buy_price;
                if (auction.auction_end_time >= DateTime.Now.AddHours(1))
                    dbAuction.auction_end_time = auction.auction_end_time;
                dbAuction.buy_now_price = auction.buy_now_price;
                dbAuction.category = auction.category;
                dbAuction.description = auction.description;
                dbAuction.item_build_year = auction.item_build_year;
                dbAuction.item_mass = auction.item_mass;
                dbAuction.condition = auction.condition;
                dbAuction.material = auction.material;
                dbAuction.username = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //dbAuction.picture = auction.picture;

                await _dataContext.SaveChangesAsync();

                return Ok(dbAuction);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating auction information: {exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(int id)
        {
            try
            {
                var auction = await _dataContext.Auctions.FindAsync(id);
                if (auction == null)
                {
                    return NoContent();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user.CollectData == true)
                    {
                        var existingRecommendation = await _dataContext.Recommendations
                        .FirstOrDefaultAsync(r => r.UserId == userId && r.AuctionId == id && r.InteractionType == InteractionType.Visit);

                        if (existingRecommendation == null)
                        {
                            var recommendation = new Recommendations
                            {
                                UserId = userId,
                                AuctionId = id,
                                CreatedAt = DateTime.Now,
                                AuctionCategory = auction.category,
                                InteractionType = InteractionType.Visit
                            };
                            _dataContext.Recommendations.Add(recommendation);
                            await _dataContext.SaveChangesAsync();
                        }
                    }
                }
                Ok(auction);
                return auction;
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Auction>>> GetAuctions([FromQuery] Models.GetAucReq req)
        {
            if (req.name == null)
            {
                var allAuctions = await _dataContext.Auctions
                    .Where(x => x.auction_end_time.AddHours(1) >= DateTime.Now)
                    .ToListAsync();
                return Ok(allAuctions);
            }
            var result = await _dataContext.Auctions.Where(req.GetPredicates()).ToListAsync();
            var activeAuctions = result.Where(x => x.auction_end_time.AddHours(1) >= DateTime.UtcNow).ToList();
            return Ok(activeAuctions);
        }



        [HttpPost("add")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<List<Auction>>> CreateAuction([FromBody] Auction auction)
        {
            if (ModelState.IsValid && auction != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);    
                var user = await _userManager.FindByIdAsync(userId);
                if(user.Bank == null && user.Paypal == null)
                {
                    return BadRequest("Need to set payout information");
                }
                auction.username = userId;
                _dataContext.Auctions.Add(auction);
                await _dataContext.SaveChangesAsync();
                return Ok(auction);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Auction>>> DeleteAuction([FromRoute] int id)
        {
            var dbAuction = await _dataContext.Auctions.FindAsync(id);
            if (dbAuction == null)
                return BadRequest("Auction not found");

            _dataContext.Remove(dbAuction);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Auctions.ToListAsync());
        }

        [HttpPost("{id}/like")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Auction>>> LikeAuction(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userId == null)
                {
                    return BadRequest("Invalid user");
                }
                var user = await _userManager.FindByIdAsync(userId);
                var auction = await _dataContext.Auctions.FindAsync(id);

                if (auction == null)
                    return NotFound("Auction not found");

                if (!auction.auction_likes_list.Contains(userId))
                {
                    auction.auction_likes_list.Add(userId);
                    auction.auction_likes = auction.auction_likes ?? 0;
                    auction.auction_likes++;
                    user.Liked_Auctions.Add(id);
                    if (user.CollectData == true)
                    {
                        var existingRecommendation = await _dataContext.Recommendations
                        .FirstOrDefaultAsync(r => r.UserId == userId && r.AuctionId == id && r.InteractionType == InteractionType.Like);

                        if (existingRecommendation == null)
                        {
                            var recommendation = new Recommendations
                            {
                                UserId = userId,
                                AuctionId = id,
                                CreatedAt = DateTime.Now,
                                AuctionCategory = auction.category,
                                InteractionType = InteractionType.Like
                            };
                            _dataContext.Recommendations.Add(recommendation);
                        }
                    }
                    await _dataContext.SaveChangesAsync();
                }
                if(!user.Liked_Auctions.Contains(id))
                {
                    user.Liked_Auctions.Add(id);
                    await _userManager.UpdateAsync(user).ConfigureAwait(false);
                }

                return Ok(auction);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while liking the auction: {exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost("{id}/unlike")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Auction>>> UnlikeAuction(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("Invalid user");
                }
                var user = await _userManager.FindByIdAsync(userId);
                var auction = await _dataContext.Auctions.FindAsync(id);

                if (auction == null)
                    return NotFound("Auction not found");

                if (auction.auction_likes_list.Contains(userId))
                {
                    auction.auction_likes_list.Remove(userId);
                    auction.auction_likes--;
                    user.Liked_Auctions.Remove(id);
                    var existingRecommendation = await _dataContext.Recommendations
                        .FirstOrDefaultAsync(r => r.UserId == userId && r.AuctionId == id && r.InteractionType == InteractionType.Like);

                    if (existingRecommendation != null)
                    {
                        _dataContext.Recommendations.Remove(existingRecommendation);
                    }
                    await _dataContext.SaveChangesAsync();
                }
                if (user.Liked_Auctions.Contains(id))
                {
                    user.Liked_Auctions.Remove(id);
                    await _userManager.UpdateAsync(user).ConfigureAwait(false);
                }
                return Ok(auction);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while unliking the auction: {exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}/comments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Comment>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Comment>>> GetCommentsForAuction(int id)
        {
            try
            {
                var auction = await _dataContext.Auctions
                    .Include(a => a.Comments)
                    .FirstOrDefaultAsync(a => a.id == id);

                if (auction == null)
                {
                    return NotFound("Auction not found");
                }

                return Ok(auction.Comments);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while fetching comments: {exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{id}/comments")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Comment))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Comment>> AddCommentToAuction(int id, [FromBody] Comment comment)
        {
            try
            {
                if (comment == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var auction = await _dataContext.Auctions
                    .Include(a => a.Comments)
                    .FirstOrDefaultAsync(a => a.id == id);

                if (auction == null)
                {
                    return NotFound("Auction not found");
                }

                var claims = User.Claims.Select(c => new { c.Type, c.Value });
                var user = await _userManager.FindByNameAsync(comment.Username);
                Console.WriteLine($"User Claims: {JsonConvert.SerializeObject(claims)}");

                comment.Date = DateTime.Now;
                comment.Username = comment.Username;
                comment.AuctionId = auction.id;

                auction.Comments.Add(comment);
                if (user.CollectData == true)
                {
                    var existingRecommendation = await _dataContext.Recommendations
                        .FirstOrDefaultAsync(r => r.UserId == user.Id && r.AuctionId == id && r.InteractionType == InteractionType.Comment);

                    if (existingRecommendation == null)
                    {
                        var recommendation = new Recommendations
                        {
                            UserId = user.Id,
                            AuctionId = id,
                            CreatedAt = DateTime.Now,
                            AuctionCategory = auction.category,
                            InteractionType = InteractionType.Comment
                        };
                        _dataContext.Recommendations.Add(recommendation);
                    }
                }
                await _dataContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCommentsForAuction), new { id = auction.id }, comment);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding a comment: {exception}", ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("upload")]
        [Authorize(Roles = ForumRoles.ForumUser)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<string>>> UploadPhoto([FromForm] IFormFileCollection files)
        {
            List<string> photoPaths = new List<string>();
            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var imageFolderPath = _configuration["ImageFolder"];
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), imageFolderPath);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        photoPaths.Add(filePath);
                    }
                }

                if (photoPaths.Count > 0)
                {
                    return Ok(photoPaths);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                foreach (var path in photoPaths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("image/{imageFilename}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetImage(string imageFilename)
        {
            try
            {
                var imageFolderPath = _configuration["ImageFolder"];
                var filePath = Path.Combine(imageFolderPath, imageFilename);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpDelete("comments/{auctionId}/{commentIndex}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteComment(int auctionId, int commentIndex)
        {
            try
            {
                var auction = await _dataContext.Auctions.FindAsync(auctionId);
                if (auction == null)
                {
                    return NotFound("Auction not found");
                }
                var comment = await _dataContext.Comments.FindAsync(commentIndex);
                if (commentIndex < 0 || comment.Id != commentIndex)
                {
                    return BadRequest("Invalid comment index");
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                if (user.UserName != comment.Username)
                {
                    return BadRequest("Invalid user");
                }
                _dataContext.Comments.Remove(comment);
                await _dataContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
        [HttpGet("suggestedauctions")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Auction>>> GetSuggestedAuctions()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notEndedAuctionsQuery = _dataContext.Auctions
                    .Where(a => a.auction_end_time > DateTime.Now);

                if (userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null && user.CollectData == true)
                    {
                        var userRecommendations = await _dataContext.UserRecommendations
                        .FirstOrDefaultAsync(r => r.UserId == userId);

                        if (userRecommendations != null && userRecommendations.RecommendedAuctionIds.Count > 0)
                        {
                            var recommendedAuctionIds = userRecommendations.RecommendedAuctionIds.Distinct().ToList();

                            var suggestedAuctions = await _dataContext.Auctions
                                .Where(a => recommendedAuctionIds.Contains(a.id))
                                .ToListAsync();

                            var indexMap = recommendedAuctionIds
                                .Select((id, index) => new { Id = id, Index = index })
                                .ToDictionary(x => x.Id, x => x.Index);

                            suggestedAuctions.Sort((a1, a2) =>
                            {
                                var index1 = indexMap[a1.id];
                                var index2 = indexMap[a2.id];
                                return index1.CompareTo(index2);
                            });

                            return Ok(suggestedAuctions);
                        }
                    }
                }

                var topAuctions = await notEndedAuctionsQuery
                    .OrderByDescending(a => a.auction_likes_list.Count)
                    .Take(5)
                    .ToListAsync();

                return Ok(topAuctions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        [HttpGet("aucccccc")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Auction))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Auction>>> GetAuctionsCC()
        {

            var query = _dataContext.Auctions.AsQueryable();
            var currentTimeUtc = DateTime.UtcNow;
            var endedAuctions = await _dataContext.Auctions
                .Where(x => currentTimeUtc > x.auction_end_time && x.auction_ended == null)
                .ToListAsync();
            return Ok(endedAuctions);
        }
    }
}