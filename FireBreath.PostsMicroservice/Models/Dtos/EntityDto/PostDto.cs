using Common.Utilities;
using static Common.Attributes.ModelAttributes;

namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string AuthorTag { get; set; }
        public string AuthorAvatar {  get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited { get; set; } = DateTime.UtcNow;
        public List<AttachmentDto?> Attachments { get; set; } = new List<AttachmentDto?>();
        public int UserId { get; set; }
        public int PostId { get; set; }

        public PostDto()
        {
            this.Id = 0;
            this.Author = string.Empty;
            this.AuthorTag = string.Empty;
            this.AuthorAvatar = string.Empty;
            this.Content = string.Empty;
            this.UserId = 0;
            this.PostId = 0;
        }
    }
}
