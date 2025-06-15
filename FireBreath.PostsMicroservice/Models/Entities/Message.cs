using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited {  get; set; } = DateTime.UtcNow;
        public List<Attachment?> Attachments { get; set; } = new List<Attachment?>();
        public int SenderId { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }

        public Message()
        {
            this.Content = string.Empty;
            this.ChatId = 0;
            this.SenderId = 0;
        }

        public Message(string content, int chatId, int senderId)
        {
            this.Content = content;
            this.ChatId = chatId;
            this.SenderId = senderId;
        }

        public Message(int id, string content, DateTime timestamp, int chatId, int senderId)
        {
            this.Id = id;
            this.Content = content;
            this.Timestamp = timestamp;
            this.ChatId = chatId;
            this.SenderId = senderId;
        }

        public Message(int id, string content, DateTime timestamp, List<Attachment?> attachments, int chatId, int senderId)
        {
            this.Id = id;
            this.Content = content;
            this.Timestamp = timestamp;
            this.Attachments = attachments;
            this.ChatId = chatId;
            this.SenderId = senderId;
        }
    }
}
