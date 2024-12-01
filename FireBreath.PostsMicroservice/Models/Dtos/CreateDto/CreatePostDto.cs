
namespace FireBreath.PostsMicroservice.Models.Dtos.CreateDto
{
    public class CreatePostDto
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public List<IFormFile?> Attachments { get; set; } = new List<IFormFile?>();
        public int UserId { get; set; }
        public int PostId { get; set; }

        public CreatePostDto()
        {
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.UserId = 0;
            this.PostId = 0;
        }
        public CreatePostDto(string author, string content, int userId)
        {
            this.Author = author;
            this.Content = content;
            this.UserId = userId;
            this.PostId = 0;
        }
        public CreatePostDto(string author, string content, List<IFormFile?> attachments, int userId)
        {
            this.Author = author;
            this.Content = content;
            this.Attachments = attachments;
            this.UserId = userId;
            this.PostId = 0;
        }
        public CreatePostDto(string author, string content, int userId, int PostId)
        {
            this.Author = author;
            this.Content = content;
            this.UserId = userId;
            this.PostId = PostId;
        }
        public CreatePostDto(string author, string content, List<IFormFile?> attachments, int userId, int postId)
        {
            this.Author= author;
            this.Content = content;
            this.UserId = userId;
            this.PostId = postId;
            this.Attachments = attachments;
        }
    }
}
