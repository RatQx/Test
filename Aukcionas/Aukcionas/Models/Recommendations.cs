namespace Aukcionas.Models
{
    public class Recommendations
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AuctionId { get; set; }
        public string AuctionCategory { get; set; }
        public DateTime CreatedAt { get; set; }
        public InteractionType InteractionType { get; set; }
    }
    public enum InteractionType
    {
        Visit = 1,
        Comment = 2,
        Bid = 3,
        Like = 4,
        Win = 5,

    }
}
