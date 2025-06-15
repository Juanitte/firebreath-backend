using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Chats")]
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string LastMessage { get; set; } = string.Empty;
        public int UnreadMessagesCount { get; set; } = 0;
        public List<int> UserIds { get; set; } = new List<int>();
        public List<Message> Messages { get; set; } = new List<Message>();

        public Chat()
        {
        }

        public Chat(string lastMessage, int unreadMessagesCount, List<int> userIds)
        {
            LastMessage = lastMessage;
            UnreadMessagesCount = unreadMessagesCount;
            UserIds = userIds ?? new List<int>();
            Messages = new List<Message>();
        }
    }
}
