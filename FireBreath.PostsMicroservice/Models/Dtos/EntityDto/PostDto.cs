using Common.Utilities;

namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AttachmentDto?> Attachments { get; set; } = new List<AttachmentDto?>();
        public int UserId { get; set; }
        public int PostId { get; set; }

        public PostDto()
        {
            this.Id = 0;
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.Timestamp = DateTime.UtcNow;
            this.UserId = 0;
            this.PostId = 0;
        }
    }
}
