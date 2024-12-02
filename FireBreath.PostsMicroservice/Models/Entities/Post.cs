using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Common.Attributes.ModelAttributes;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Filters]
        public string Author { get; set; }
        [Filters]
        public string Content { get; set; }
        [Filters]
        public DateTime Timestamp { get; set; }
        public List<Attachment?> AttachmentPaths { get; set; } = new List<Attachment?>();
        public int UserId { get; set; }
        public int PostId { get; set; }

        public Post()
        {
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.Timestamp = DateTime.Now;
            this.UserId = 0;
            this.PostId = 0;
        }

        public Post(string content, string author, int userId, int postId)
        {
            this.Author = author;
            this.Content = content;
            this.Timestamp = DateTime.Now;
            this.UserId = userId;
            this.PostId = postId;
        }
    }
}
