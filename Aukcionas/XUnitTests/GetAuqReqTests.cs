using Aukcionas.Models;
using Aukcionas.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTests
{
    public class GetAuqReqTests
    {
        [Fact]
        public void GetPredicates_WithName_ReturnsCorrectPredicate()
        {
            var request = new GetAucReq { name = "Test" };
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var auction = new Auction { name = "Test Name", auction_end_time = DateTime.UtcNow.AddHours(2) };
            var result = compiledPredicate(auction);
            Assert.True(result);
        }

        [Fact]
        public void GetPredicates_WithoutName_ReturnsDefaultPredicate()
        {
            var request = new GetAucReq();
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var auction = new Auction { name = "Test auction", auction_end_time = DateTime.UtcNow.AddHours(2) };
            var result = compiledPredicate(auction);
            Assert.True(result);
        }

        [Fact]
        public void GetPredicates_AuctionNotEndingSoon_ReturnsTrue()
        {
            var request = new GetAucReq { name = "Test" };
            var auction = new Auction { name = "Test Name", auction_end_time = DateTime.UtcNow.AddHours(2) };
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var result = compiledPredicate(auction);
            Assert.True(result);
        }

        [Fact]
        public void GetPredicates_WithNameContainingSpecialCharacters_ReturnsCorrectPredicate()
        {
            var request = new GetAucReq { name = "$$$" };
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var auction = new Auction { name = "$$$ Special Auction $$$", auction_end_time = DateTime.UtcNow.AddHours(2) };
            var result = compiledPredicate(auction);
            Assert.True(result);
        }

        [Fact]
        public void GetPredicates_AuctionEndingSoon_ReturnsFalse()
        {
            var request = new GetAucReq { name = "Test" };
            var auction = new Auction { name = "Test Name", auction_end_time = DateTime.UtcNow.AddMinutes(30) };
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var result = compiledPredicate(auction);
            Assert.False(result);
        }

        [Fact]
        public void GetPredicates_AuctionAlreadyEnded_ReturnsFalse()
        {
            var request = new GetAucReq { name = "Test" };
            var auction = new Auction { name = "Test Auction", auction_end_time = DateTime.UtcNow.AddHours(-1) };
            var predicate = request.GetPredicates();
            var compiledPredicate = predicate.Compile();
            var result = compiledPredicate(auction);
            Assert.False(result);
        }
    }
}
