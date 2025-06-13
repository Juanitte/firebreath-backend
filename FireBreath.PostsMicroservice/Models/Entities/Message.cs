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
        public int ReceiverId {  get; set; }

        public Message()
        {
            this.Content = string.Empty;
            this.SenderId = 0;
            this.ReceiverId = 0;
        }

        public Message(string content, int senderId, int receiverId)
        {
            this.Content = content;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }

        public Message(int id, string content, DateTime timestamp, int senderId, int receiverId)
        {
            this.Id = id;
            this.Content = content;
            this.Timestamp = timestamp;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }

        public Message(int id, string content, DateTime timestamp, List<Attachment?> attachments, int senderId, int receiverId)
        {
            this.Id = id;
            this.Content = content;
            this.Timestamp = timestamp;
            this.Attachments = attachments;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }
    }
}
