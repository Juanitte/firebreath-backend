using FireBreath.PostsMicroservice.Models.Dtos.EntityDto;

namespace FireBreath.PostsMicroservice.Models.Dtos.CreateDto
{
    public class CreateChatDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();

        public CreateChatDto()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public CreateChatDto(List<int> userIds)
        {
            UserIds = userIds ?? new List<int>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
