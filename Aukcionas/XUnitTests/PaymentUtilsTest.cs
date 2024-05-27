using Aukcionas.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTests
{
    public class PaymentUtilsTest
    {
        private readonly IConfiguration _configuration;

        public PaymentUtilsTest()
        {
            var configurationMock = new Mock<IConfiguration>();
            var hmac = new HMACSHA256();
            var keyBytes = hmac.Key;
            var base64Key = Convert.ToBase64String(keyBytes);
            configurationMock.Setup(x => x["JWT:Secret"]).Returns(base64Key);
            _configuration = configurationMock.Object;
        }

        [Fact]
        public void GeneratePaymentToken_ReturnsValidToken()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            var userId = "123";
            var auctionId = 456;
            var token = paymentUtils.GeneratePaymentToken(userId, auctionId);
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            var (decodedUserId, decodedAuctionId) = paymentUtils.DecodePaymentToken(token);
            Assert.Equal(userId, decodedUserId);
            Assert.Equal(auctionId, decodedAuctionId);
        }


        [Fact]
        public void DecodePaymentToken_ValidToken_ReturnsDecodedValues()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            var userId = "user1234567890";
            var auctionId = 456;
            var token = paymentUtils.GeneratePaymentToken(userId, auctionId);
            var (decodedUserId, decodedAuctionId) = paymentUtils.DecodePaymentToken(token);
            Assert.Equal(userId, decodedUserId);
            Assert.Equal(auctionId, decodedAuctionId);
        }


        [Fact]
        public void DecodePaymentToken_InvalidToken_ThrowsException()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            var invalidToken = "invalidtoken";
            Assert.Throws<ArgumentException>(() => paymentUtils.DecodePaymentToken(invalidToken));
        }
        [Fact]
        public void DecodePaymentToken_NullToken_ThrowsArgumentNullException()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            var ex = Assert.Throws<ArgumentException>(() => paymentUtils.DecodePaymentToken(null));
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }
        [Fact]
        public void DecodePaymentToken_EmptyToken_ThrowsArgumentException()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            Assert.Throws<ArgumentException>(() => paymentUtils.DecodePaymentToken(""));
        }

        [Fact]
        public void DecodePaymentToken_ExpiredToken_ThrowsException()
        {
            var paymentUtils = new PaymentUtils(_configuration);
            var userId = "123";
            var auctionId = 456;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secure_random_key_secure_random_key_secure_random_key"));
            var pastExpirationDate = DateTime.UtcNow.AddMinutes(-30);
            var validNotBeforeDate = DateTime.UtcNow.AddMinutes(-60);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId), new Claim("AuctionId", auctionId.ToString()) }),
                Expires = pastExpirationDate,
                NotBefore = validNotBeforeDate,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var expiredToken = tokenHandler.CreateToken(tokenDescriptor);
            var expiredTokenString = tokenHandler.WriteToken(expiredToken);
            Assert.Throws<ArgumentException>(() => paymentUtils.DecodePaymentToken(expiredTokenString));
        }
    }
}
