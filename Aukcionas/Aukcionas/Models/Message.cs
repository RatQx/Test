namespace Aukcionas.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
    }
}
