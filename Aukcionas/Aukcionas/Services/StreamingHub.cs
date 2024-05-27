using Aukcionas.Data;
using Aukcionas.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Aukcionas.Services
{
    public class StreamingHub : Hub
    {
        private readonly DataContext _dataContext;
        public StreamingHub(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task StartLiveStream(int auction_id, string user)
        {
            try
            {
                var fetch_user = _dataContext.Users.FirstOrDefault(u => u.UserName == user);
                if (user == null)
                {
                    return;
                }
                var auction = _dataContext.Auctions.Find(auction_id);
                if (auction == null)
                {
                    return;
                }
                if (auction.username == fetch_user.Id.ToString())
                {
                    await Clients.All.SendAsync("LiveStreamStarted");
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error starting live stream: " + ex.Message);
            }
        }

        public async Task StopLiveStream(int auction_id, string user)
        {
            try
            {
                var fetch_user = _dataContext.Users.FirstOrDefault(u => u.UserName == user);
                if (user == null)
                {
                    return;
                }
                var auction = _dataContext.Auctions.Find(auction_id);
                if (auction == null)
                {
                    return;
                }
                if (auction.username == fetch_user.Id.ToString())
                {
                    await Clients.All.SendAsync("LiveStreamStopped");
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping live stream: " + ex.Message);
            }
        }

        public async Task FinishLiveStream(int auction_id, string user)
        {
            try
            {
                var fetch_user = _dataContext.Users.FirstOrDefault(u => u.UserName == user);
                if (user == null)
                {
                    return;
                }
                var auction = _dataContext.Auctions.Find(auction_id);
                if (auction == null)
                {
                    return;
                }
                if (auction.username == fetch_user.Id.ToString())
                {
                    await Clients.All.SendAsync("LiveStreamFinished");
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finishing live stream: " + ex.Message);
            }
        }
    }
}
