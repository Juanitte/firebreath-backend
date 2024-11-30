using EasyWeb.UserMicroservice.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EasyWeb.TicketsMicroservice.Models.Entities;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Attachment?> AttachmentPaths { get; set; } = new List<Attachment?>();
        public int UserId { get; set; }
        public int PostId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("PostId")]
        public Post? PostRef { get; set; }

        public Post()
        {
            Author = string.Empty;
            Content = string.Empty;
            this.Timestamp = DateTime.Now;
            UserId = 0;
            User = null;
            PostId = 0;
            PostRef = null;
        }

        public Post(string content, string author, int userId, int postId)
        {
            Author = author;
            Content = content;
            this.Timestamp = DateTime.Now;
            UserId = userId;
            PostId = postId;
            User = null;
            PostRef = null;
        }
    }
}
