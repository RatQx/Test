namespace Aukcionas.Models
{
    public class UserRecommendations
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<int> RecommendedAuctionIds { get; set; }
    }
}
