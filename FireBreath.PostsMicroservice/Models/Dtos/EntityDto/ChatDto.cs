using FireBreath.PostsMicroservice.Models.Entities;

namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class ChatDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string LastMessage { get; set; }
        public int UnreadMessagesCount { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();

        public ChatDto()
        {
            Id = 0;
            LastMessage = string.Empty;
            UnreadMessagesCount = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public ChatDto(int id, DateTime createdAt, DateTime updatedAt, string lastMessage, int unreadMessagesCount, List<int> userIds, List<MessageDto> messages)
        {
            Id = id;
            LastMessage = lastMessage;
            UnreadMessagesCount = unreadMessagesCount;
            UserIds = userIds ?? new List<int>();
            Messages = messages ?? new List<MessageDto>();
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
