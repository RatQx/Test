using Aukcionas.Models;

namespace Aukcionas.Utils
{
    public static class EmailBody
    {
        public static string EmailStringReset(string email, string emailToken)
        {
            return $@"<html>
            <body>
                <div>
                    <p>Reset your password.</p>
                    <P>Click link below to reset your password. Link will expire in 15 minutes.</p><br>
                    <a href=http://localhost:4200/reset?email={email}&code={emailToken}>Reset password</a><br>
                </div>
            </body> 
            </html>
            ";
        }

        public static string EmailStringConfirm(string email, string emailToken)
        {
            return $@"<html>
        <body>
            <div>
                <p>Welcome.</p>
                <P>Click the link below to confirm your email address:</p><br>
                <a href=http://localhost:4200/confirm-email?email={email}&code={emailToken}>Confirm Email</a><br>
            </div>
        </body> 
        </html>
        ";
        }

        public static string EmailPaymentForUser(string paymentToken, string auctionName, double price)
        {
            return $@"<html>
        <body>
            <div>
                <p>Congratulations!</p>
                <P>You won online auction: {auctionName} </p><br>
                <P>You outbid other contestants and won the auction. Your bid price was: {price} </p><br>
                <p>Click the link below to make payment for the auction. The link will be valid for 1 week.</p><br>
                <a href=http://localhost:4200/payment?token={paymentToken}>Payment link</a><br>
            </div>
        </body> 
        </html>
        ";
        }

        public static string EmailAuctionWonForOwner(string auctionName, double price, string auction_winner, int auction_id)
        {
            return $@"<html>
        <body>
            <div>
                <p>Congratulations!</p>
                <P>Your auction: {auctionName}, has finished</p><br>
                <P>The bids were enough to meet the minimum buy amount. The winning bid was: {price} </p><br>
                <P>User that won the auction: {auction_winner}</p><br>
                <p>This user now has 1 week to make payment for the auction.</p><br>
                <p>Once the payment is complete, you will receive the user's shipping information.</p><br>
                <p>For more information, check the auction page:</p>
                <a href=http://localhost:4200/auction-bid/{auction_id}>Auction page</a><br>
            </div>
        </body> 
        </html>
        ";
        }

        public static string EmailAuctionEndedForOwner(string auctionName, int auction_id)
        {
            string auctionNameText = !string.IsNullOrEmpty(auctionName) ? auctionName : "Auction";
            return $@"<html>
        <body>
            <div>
                <P>Your auction: {auctionNameText}, has finished</p><br>
                <P>The bids were not enough to meet the minimum buy amount. </p><br>
                <p>For more information, check your auction page:</p>
                <a href=http://localhost:4200/auction-bid/{auction_id}>Auction page</a><br>
            </div>
        </body> 
        </html>
        ";
        }

        public static string EmailAuctionOwnerOnPayment(string auction_email, Payment payment)
        {
            if (payment == null)
            {
                return $@"<html>
            <body>
                <div>
                    <P>Payment information on auction</p>
                    <P>Your auction payment was unsuccessful. Payment details are not available.</p>
                    <p>For more information, please contact us: {auction_email}</p>
                </div>
            </body> 
            </html>
            ";
            }

            return $@"<html>
        <body>
            <div>
                <P>Payment information on auction</p>
                <P>Your auction payment was successful. Open the link below to view your auction:</p>
                <a href=http://localhost:4200/auction-bid/{payment.Auction_Id}>Auction page</a>
                <p>Funds for the auction will soon be transferred to your account.</p></br>
                <p>This is the user's shipping address:</p>
                <p>Address: {payment.Address_Line1}</p>
                <p>Address line 2 (optional): {payment.Address_Line2}</p>
                <p>Country: {payment.Country}</p>
                <p>Postal code: {payment.Postal_Code}</p>
                <p>Payment amount: {payment.Payment_Amount}</p>
                <p>Payment time: {payment.Payment_Time}</p>
                <p>For more information, please contact us: {auction_email}</p>
            </div>
        </body> 
        </html>
        ";
        }

        public static string EmailAuctionWinnerOnPayment(string auction_email, Payment payment)
        {
            if (payment == null)
            {
                return $@"<html>
            <body>
                <div>
                    <P>This is your payment information.</p>
                    <P>Your payment details are not available.</p>
                    <p>For more information, please contact us: {auction_email}</p>
                </div>
            </body> 
            </html>
            ";
            }

            return $@"<html>
        <body>
            <div>
                <P>Payment information</p>
                <P>This is your payment information.</p>
                <p>Address: {payment.Address_Line1}</p>
                <p>Address line 2 (optional): {payment.Address_Line2}</p>
                <p>Country: {payment.Country}</p>
                <p>Postal code: {payment.Postal_Code}</p>
                <p>Payment amount: {payment.Payment_Amount}</p>
                <p>Payment time: {payment.Payment_Time}</p>
                <p>For more information, please contact us: {auction_email}</p>
            </div>
        </body> 
        </html>
        ";
        }
    }
}
