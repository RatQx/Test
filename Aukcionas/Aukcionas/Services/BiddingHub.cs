using Aukcionas.Data;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using Aukcionas.Auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Aukcionas.Models;
using Microsoft.EntityFrameworkCore;

namespace Aukcionas.Services
{
    public class BiddingHub : Hub
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<ForumRestUser> _userManager;
        private readonly ILogger<BiddingHub> _logger;
        public BiddingHub(DataContext dataContext, ILogger<BiddingHub> logger, UserManager<ForumRestUser> userManager)
        {
            _dataContext = dataContext;
            _logger = logger;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "AuctionBid");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "AuctionBid");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task PlaceBid(int auctionId, double bidAmount, string username)
        {
            try
            {
                var auction = _dataContext.Auctions.Find(auctionId);
                if (auction == null ||auction.auction_ended == true || auction.auction_end_time <= DateTime.Now)
                {
                    return;
                }
                if (string.IsNullOrEmpty(username))
                {
                    return;
                }
                var user = await _userManager.FindByNameAsync(username);
                if(user == null)
                {
                    return;
                }
                double currentBid = (auction.bidding_amount_history.Count > 0)
                    ? auction.bidding_amount_history.Last()
                    : auction.starting_price;

                if (bidAmount >= currentBid)
                {
                    if (auction.auction_biders_list == null)
                    {
                        auction.auction_biders_list = new List<string>();
                    }
                    auction.auction_biders_list.Add(username);

                    if (auction.bidding_amount_history == null)
                    {
                        auction.bidding_amount_history = new List<double>();
                    }
                    auction.bidding_amount_history.Add(bidAmount);

                    if (auction.bidding_times_history == null)
                    {
                        auction.bidding_times_history = new List<DateTime>();
                    }
                    auction.bidding_times_history.Add(DateTime.Now);

                    await Clients.All.SendAsync("UpdateBid", auctionId, bidAmount, username);

                    if (user.CollectData == true)
                    {
                        var existingRecommendation = await _dataContext.Recommendations
                            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.AuctionId == auctionId && r.InteractionType == InteractionType.Bid);

                        if (existingRecommendation == null)
                        {
                            var recommendation = new Recommendations
                            {
                                UserId = user.Id,
                                AuctionId = auctionId,
                                CreatedAt = DateTime.Now,
                                AuctionCategory = auction.category,
                                InteractionType = InteractionType.Bid
                            };
                            _dataContext.Recommendations.Add(recommendation);
                        }
                    }
                }
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing bid.");
                throw;
            }
        }
    }
}
