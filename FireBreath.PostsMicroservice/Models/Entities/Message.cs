using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EasyWeb.UserMicroservice.Models.Entities;

namespace EasyWeb.TicketsMicroservice.Models.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Attachment?> AttachmentPaths { get; set; } = new List<Attachment?>();
        public int SenderId { get; set; }
        public int ReceiverId {  get; set; }
        [ForeignKey("SenderId")]
        public User? Sender { get; set; }
        [ForeignKey("ReceiverId")]
        public User? Receiver { get; set; }

        public Message()
        {
            Author = string.Empty;
            Content = string.Empty;
            this.Timestamp = DateTime.Now;
            SenderId = 0;
            Sender = null;
            ReceiverId = 0;
            Receiver = null;
        }

        public Message(string content, string author, int senderId, int receiverId)
        {
            Author = author;
            Content = content;
            this.Timestamp = DateTime.Now;
            SenderId = senderId;
            Sender = null;
            ReceiverId = receiverId;
            Receiver = null;
        }
    }
}
