using Aukcionas.Auth.Model;

namespace Aukcionas.Models
{
    public class PaymentLinks
    {
        public int Id { get; set; }
        public string Payment_Link { get; set; }
        public int AuctionId { get; set; }

        public string UserId { get; set; }
        public ForumRestUser User { get; set; }
    }
}
