using Aukcionas.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aukcionas.Models;

namespace XUnitTests
{
    public class EmailBodyTests
    {
        [Fact]
        public void EmailStringReset_ValidInput_GenerateCorrectEmailBody()
        {
            string email = "user@example.com";
            string emailToken = "token123";
            string emailBody = EmailBody.EmailStringReset(email, emailToken);
            Assert.Contains("Reset your password.", emailBody);
            Assert.Contains($"reset?email={email}&code={emailToken}", emailBody);
        }

        [Fact]
        public void EmailStringReset_NullInput_HandleError()
        {
            string email = null;
            string emailToken = null;
            string emailBody = EmailBody.EmailStringReset(email, emailToken);
            Assert.NotNull(emailBody);
            Assert.Contains("reset?email=", emailBody);
        }

        [Fact]
        public void EmailStringReset_EmptyInput_GenerateDefaultBody()
        {
            string email = "";
            string emailToken = "";
            string emailBody = EmailBody.EmailStringReset(email, emailToken);
            Assert.Contains("Reset your password.", emailBody);
            Assert.Contains("reset?email=", emailBody);
        }
        [Fact]
        public void EmailStringConfirm_ValidInput_GenerateCorrectEmailBody()
        {
            string email = "user@example.com";
            string emailToken = "token123";
            string emailBody = EmailBody.EmailStringConfirm(email, emailToken);
            Assert.Contains("Click the link below to confirm your email address:", emailBody);
            Assert.Contains($"confirm-email?email={email}&code={emailToken}", emailBody);
        }

        [Fact]
        public void EmailStringConfirm_NullInput_HandleError()
        {
            string email = null;
            string emailToken = null;
            string emailBody = EmailBody.EmailStringConfirm(email, emailToken);
            Assert.NotNull(emailBody);
            Assert.Contains("confirm-email?email=", emailBody);
        }

        [Fact]
        public void EmailStringConfirm_EmptyInput_GenerateDefaultBody()
        {
            string email = "";
            string emailToken = "";
            string emailBody = EmailBody.EmailStringConfirm(email, emailToken);
            Assert.Contains("Click the link below to confirm your email address:", emailBody);
            Assert.Contains("confirm-email?email=", emailBody);
        }

        [Fact]
        public void EmailPaymentForUser_ValidInput_ShouldGenerateCorrectEmailBody()
        {
            string paymentToken = "paymentToken123";
            string auctionName = "Auction";
            double price = 100.0;
            string emailBody = EmailBody.EmailPaymentForUser(paymentToken, auctionName, price);
            Assert.Contains($"You won online auction: {auctionName}", emailBody);
            Assert.Contains($"You outbid other contestants and won the auction. Your bid price was: {price}", emailBody);
            Assert.Contains($"payment?token={paymentToken}", emailBody);
        }

        [Fact]
        public void EmailPaymentForUser_NullAuctionName_HandleError()
        {
            string paymentToken = "paymentToken123";
            string auctionName = null;
            double price = 200.0;
            string emailBody = EmailBody.EmailPaymentForUser(paymentToken, auctionName, price);
            Assert.NotNull(emailBody);
            Assert.Contains($"You won online auction:", emailBody);
            Assert.Contains($"You outbid other contestants and won the auction. Your bid price was: {price}", emailBody);
            Assert.Contains($"Click the link below to make payment for the auction. The link will be valid for 1 week.", emailBody);
            Assert.Contains($"payment?token={paymentToken}", emailBody);
        }

        [Fact]
        public void EmailPaymentForUser_ZeroPrice_GenerateDefaultBody()
        {
            string paymentToken = "paymentToken123";
            string auctionName = "Auction";
            double price = 0.0;
            string emailBody = EmailBody.EmailPaymentForUser(paymentToken, auctionName, price);
            Assert.Contains($"You won online auction: {auctionName}", emailBody);
            Assert.DoesNotContain("Your bidded price was:", emailBody);
        }
        [Fact]
        public void EmailAuctionWonForOwner_ValidInput_GenerateCorrectEmailBody()
        {
            string auctionName = "Auction";
            double price = 200.0;
            string winnerName = "John Doe";
            int auctionId = 123;
            string emailBody = EmailBody.EmailAuctionWonForOwner(auctionName, price, winnerName, auctionId);
            Assert.NotNull(emailBody);
            Assert.Contains($"Your auction: {auctionName}, has finished", emailBody);
            Assert.Contains($"The winning bid was: {price}", emailBody);
            Assert.Contains($"User that won the auction: {winnerName}", emailBody);
            Assert.Contains($"auction-bid/{auctionId}", emailBody);
        }

        [Fact]
        public void EmailAuctionWonForOwner_NullWinnerName_HandleError()
        {
            string auctionName = "Auction";
            double price = 150.0;
            string winnerName = null;
            int auctionId = 456;
            string emailBody = EmailBody.EmailAuctionWonForOwner(auctionName, price, winnerName, auctionId);
            Assert.NotNull(emailBody);
            Assert.Contains($"Your auction: {auctionName}, has finished", emailBody);
            Assert.Contains($"The winning bid was: {price}", emailBody);
            Assert.Contains($"User that won the auction:", emailBody);
            Assert.Contains($"auction-bid/{auctionId}", emailBody);
        }
        [Fact]
        public void EmailAuctionEndedForOwner_ValidInput_GenerateCorrectEmailBody()
        {
            string auctionName = "Auction";
            int auctionId = 123;
            string emailBody = EmailBody.EmailAuctionEndedForOwner(auctionName, auctionId);
            Assert.NotNull(emailBody);
            Assert.StartsWith("<html>", emailBody);
            Assert.Contains("<body>", emailBody);
            Assert.Contains("<div>", emailBody);
            string expectedAuctionNameMessage = $"Your auction: {auctionName}, has finished";
            Assert.Contains(expectedAuctionNameMessage, emailBody);
            string expectedBidsMessage = "The bids were not enough to meet the minimum buy amount.";
            Assert.Contains(expectedBidsMessage, emailBody);
            string expectedAuctionPageLink = $"<a href=http://localhost:4200/auction-bid/{auctionId}>Auction page</a>";
            Assert.Contains(expectedAuctionPageLink, emailBody);
        }

        [Fact]
        public void EmailAuctionEndedForOwner_NullAuctionName_HandleError()
        {
            string auctionName = null;
            int auctionId = 456;
            string emailBody = EmailBody.EmailAuctionEndedForOwner(auctionName, auctionId);
            Assert.NotNull(emailBody);
            Assert.DoesNotContain("You auction: ", emailBody);
            Assert.Contains("has finished", emailBody);
            Assert.Contains($"auction-bid/{auctionId}", emailBody);
        }
        [Fact]
        public void EmailAuctionOwnerOnPayment_ValidInput_GenerateCorrectEmailBody()
        {
            string auctionEmail = "owner@example.com";
            Payment payment = new Payment
            {
                Auction_Id = "123",
                Address_Line1 = "123 Street",
                Address_Line2 = "Srt 2",
                Country = "EUR",
                Postal_Code = "12345",
                Payment_Amount = 200.0,
                Payment_Time = DateTime.Now
            };
            string emailBody = EmailBody.EmailAuctionOwnerOnPayment(auctionEmail, payment);
            Assert.Contains($"Your auction payment was successful.", emailBody);
            Assert.Contains($"Address: {payment.Address_Line1}", emailBody);
            Assert.Contains($"Country: {payment.Country}", emailBody);
            Assert.Contains($"Postal code: {payment.Postal_Code}", emailBody);
            Assert.Contains($"auction-bid/{payment.Auction_Id}", emailBody);
        }

        [Fact]
        public void EmailAuctionOwnerOnPayment_NullPayment_HandleError()
        {
            string auctionEmail = "owner@example.com";
            Payment payment = null;
            string emailBody = EmailBody.EmailAuctionOwnerOnPayment(auctionEmail, payment);
            Assert.NotNull(emailBody);
            Assert.DoesNotContain("Address:", emailBody);
        }
        [Fact]
        public void EmailAuctionWinnerOnPayment_ValidInput_GenerateCorrectEmailBody()
        {
            string auctionEmail = "winner@example.com";
            Payment payment = new Payment
            {
                Address_Line1 = "123 Str",
                Address_Line2 = "",
                Country = "LT",
                Postal_Code = "12345",
                Payment_Amount = 150.0,
                Payment_Time = DateTime.Now
            };
            string emailBody = EmailBody.EmailAuctionWinnerOnPayment(auctionEmail, payment);
            Assert.Contains($"This is your payment information.", emailBody);
            Assert.Contains($"Address: {payment.Address_Line1}", emailBody);
            Assert.Contains($"Country: {payment.Country}", emailBody);
            Assert.Contains($"Postal code: {payment.Postal_Code}", emailBody);
        }

        [Fact]
        public void EmailAuctionWinnerOnPayment_NullPayment_HandleError()
        {
            string auctionEmail = "winner@example.com";
            Payment payment = null;
            string emailBody = EmailBody.EmailAuctionWinnerOnPayment(auctionEmail, payment);
            Assert.NotNull(emailBody);
            Assert.DoesNotContain("Address:", emailBody);
        }
    }
}
