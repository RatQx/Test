using Aukcionas.Auth.Model;
using Aukcionas.Data;
using Aukcionas.Models;
using Aukcionas.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Aukcionas.Services
{
    public class AuctionStatusService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public AuctionStatusService(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task CheckAuctionStatus(DataContext dbContext)
        {
            Console.WriteLine("Checking auction status.. date now {0}" , DateTime.Now);
            //var currentTimeUtc = DateTime.UtcNow;
            //var endedAuctions = await dbContext.Auctions
            //    .Where(x => currentTimeUtc > x.auction_end_time && x.auction_ended == null)
            //    .ToListAsync();
            var endedAuctions = new List<Auction>();
            Console.WriteLine("Finished checking..");
            foreach (var auction in endedAuctions)
            {
                auction.auction_ended = true;
                var userManager = dbContext.GetService<UserManager<ForumRestUser>>();
                if (auction.bidding_amount_history.Any())
                {
                    double highestBid = auction.bidding_amount_history.Max();
                    if (highestBid >= auction.min_buy_price)
                    {
                        auction.auction_won = true;
                        var user = await userManager.FindByNameAsync(auction.auction_biders_list.Last());
                        auction.auction_winner = user.UserName;
                        user.Auctions_Won.Add(auction.id);
                        if(user.CollectData == true)
                        {
                            var existingRecommendation = await dbContext.Recommendations
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
                                dbContext.Recommendations.Add(recommendation);
                            }
                        }

                        var auctionOwner = await userManager.FindByIdAsync(auction.username);
                            
                        var payment = new PaymentUtils(_configuration);
                        var emailToken = payment.GeneratePaymentToken(user.Id,auction.id);
                        var paymentLink = new PaymentLinks
                        {
                            Payment_Link = $"http://localhost:4200/payment?token={emailToken}",
                            AuctionId = auction.id,
                            UserId = user.Id
                        };
                        dbContext.PaymentLinks.Add(paymentLink);

                        string from = _configuration["EmailConfiguration:From"];
                        var emailModelBuyer = new EmailModel(user.Email, "Auction win", EmailBody.EmailPaymentForUser(emailToken, auction.name, auction.bidding_amount_history.Last()), "Auction winner confirmation");
                        _emailService.SendEmail(emailModelBuyer);

                        var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has a winner", EmailBody.EmailAuctionWonForOwner(auction.name, auction.bidding_amount_history.Last(), user.UserName, auction.id), "Auction ended");
                        _emailService.SendEmail(emailModelOwner);
                    }
                    else
                    {
                        var auctionOwner = await userManager.FindByIdAsync(auction.username);
                        var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has ended", EmailBody.EmailAuctionEndedForOwner(auction.name, auction.id), "Auction ended");
                        _emailService.SendEmail(emailModelOwner);
                    }
                }
                else
                {
                    var auctionOwner = await userManager.FindByIdAsync(auction.username);
                    var emailModelOwner = new EmailModel(auctionOwner.Email, "Your auction has ended", EmailBody.EmailAuctionEndedForOwner(auction.name, auction.id), "Auction ended");
                    _emailService.SendEmail(emailModelOwner);
                }

                await dbContext.SaveChangesAsync();
            }
            await dbContext.SaveChangesAsync();
        }

    }
}
