using Aukcionas.Auth.Model;
using Aukcionas.Data;
using Aukcionas.Models;
using Aukcionas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace XUnitTests
{
    public class BiddingHubTests
    {
        [Fact]
        public async Task PlaceBid_ValidBid_Success()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();
                var auction = new Auction
                {
                    id = 1,
                    category = "Test",
                    city = "Kaunas",
                    condition = "New",
                    country = "LT",
                    description = "Test",
                    item_build_year = "2000",
                    material = "Metal",
                    name = "TestName"
                };
                context.Auctions.Add(auction);
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(1, 100.0, "testuser");
                await context.SaveChangesAsync();
                Assert.NotNull(context.Auctions.FirstOrDefault(a => a.id == 1));
            }
        }
        [Fact]
        public async Task PlaceBid_InvalidAuctionId_ReturnsError()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(999, 100.0, "testuser");
                Assert.Null(context.Auctions.FirstOrDefault(a => a.id == 999));
            }
        }
        [Fact]
        public async Task PlaceBid_ExpiredAuction_ReturnsError()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();
                var auction = new Auction
                {
                    id = 2,
                    category = "Test",
                    city = "Kaunas",
                    condition = "New",
                    country = "LT",
                    description = "Test",
                    item_build_year = "2000",
                    material = "Metal",
                    name = "TestName",
                    auction_ended = true,
                    auction_end_time = DateTime.Now.AddHours(-1)
                };
                context.Auctions.Add(auction);
                await context.SaveChangesAsync();
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(1, 100.0, "testuser");
                Assert.DoesNotContain("testuser", auction.auction_biders_list);
                Assert.DoesNotContain(100.0, auction.bidding_amount_history);
            }
        }
        [Fact]
        public async Task PlaceBid_LowerBidAmount_ReturnsError()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();
                var auction = new Auction
                {
                    id = 3,
                    category = "Test",
                    city = "Kaunas",
                    condition = "New",
                    country = "LT",
                    description = "Test",
                    item_build_year = "2000",
                    material = "Metal",
                    name = "TestName",
                    starting_price = 100.0,
                    bidding_amount_history = new List<double> { 150.0, 200.0 },
                    auction_end_time = DateTime.Now.AddHours(1)
                };
                context.Auctions.Add(auction);
                await context.SaveChangesAsync();
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(3, 80.0, "testuser");
                Assert.DoesNotContain("testuser", auction.auction_biders_list);
                Assert.DoesNotContain(80.0, auction.bidding_amount_history);
            }
        }
        [Fact]
        public async Task PlaceBid_InvalidUsername_ReturnsError()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(4, 150.0, "");
                Assert.Null(context.Auctions.FirstOrDefault(a => a.id == 4));
            }
        }

        [Fact]
        public async Task PlaceBid_InvalidUsername_NoActionTaken()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(options))
            {
                var loggerMock = new Mock<ILogger<BiddingHub>>();
                var userManagerMock = MockUserManager();

                var auction = new Auction
                {
                    id = 6,
                    category = "Test",
                    city = "Kaunas",
                    condition = "New",
                    country = "LT",
                    description = "Test",
                    item_build_year = "2000",
                    material = "Metal",
                    name = "TestName",
                    auction_end_time = DateTime.Now.AddHours(1)
                };

                context.Auctions.Add(auction);
                await context.SaveChangesAsync();
                var biddingHub = new BiddingHub(context, loggerMock.Object, userManagerMock);
                await biddingHub.PlaceBid(6, 100.0, "");
                Assert.Empty(auction.auction_biders_list);
                Assert.Empty(auction.bidding_amount_history);
                var recommendation = await context.Recommendations.FirstOrDefaultAsync(r => r.AuctionId == 6);
                Assert.Null(recommendation);
            }
        }

        private UserManager<ForumRestUser> MockUserManager()
        {
            var users = new List<ForumRestUser>
            {
                 new ForumRestUser { Id = "1", UserName = "testuser", CollectData = true },
                new ForumRestUser { Id = "2", UserName = "testuser2" }
            };
            var userStoreMock = new Mock<IUserStore<ForumRestUser>>();
            var userManagerMock = new Mock<UserManager<ForumRestUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            userManagerMock.Setup(u => u.FindByNameAsync(It.IsAny<string>()))
                           .ReturnsAsync((string userName) => users.FirstOrDefault(u => u.UserName == userName));

            return userManagerMock.Object;
        }
    }
}