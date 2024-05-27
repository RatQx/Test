namespace Aukcionas.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public string CreatedUserName { get; set; }
        public string ReciverUser { get; set; }
        public string ReciverUserName { get; set; }
    }
}
