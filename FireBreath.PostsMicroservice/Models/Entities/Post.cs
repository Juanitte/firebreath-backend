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
        public string AuthorTag { get; set; }
        [Filters]
        public string Content { get; set; }
        [Filters]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited { get; set; } = DateTime.UtcNow;
        public List<Attachment?> Attachments { get; set; } = new List<Attachment?>();
        public int UserId { get; set; }
        public int PostId { get; set; }

        public Post()
        {
            this.Author = string.Empty;
            this.AuthorTag = string.Empty;
            this.Content = string.Empty;
            this.UserId = 0;
            this.PostId = 0;
        }

        public Post(string content, string author, string authorTag, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.Content = content;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string content, DateTime created, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.Content = content;
            this.Created = created;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string content, List<Attachment?> attachments, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.Content = content;
            this.Attachments = attachments;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string content, DateTime created, List<Attachment?> attachments, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.Content = content;
            this.Created = created;
            this.Attachments = attachments;
            this.UserId = userId;
            this.PostId = postId;
        }
    }
}
