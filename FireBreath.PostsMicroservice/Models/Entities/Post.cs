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
        public string AuthorAvatar {  get; set; }
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
            this.AuthorAvatar = string.Empty;
            this.Content = string.Empty;
            this.UserId = 0;
            this.PostId = 0;
        }

        public Post(string author, string authorTag, string authorAvatar, string content, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.AuthorAvatar = authorAvatar;
            this.Content = content;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string authorAvatar, string content, DateTime created, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.AuthorAvatar = authorAvatar;
            this.Content = content;
            this.Created = created;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string authorAvatar, string content, List<Attachment?> attachments, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.AuthorAvatar = authorAvatar;
            this.Content = content;
            this.Attachments = attachments;
            this.UserId = userId;
            this.PostId = postId;
        }

        public Post(string author, string authorTag, string authorAvatar, string content, DateTime created, List<Attachment?> attachments, int userId, int postId)
        {
            this.Author = author;
            this.AuthorTag = authorTag;
            this.AuthorAvatar = authorAvatar;
            this.Content = content;
            this.Created = created;
            this.Attachments = attachments;
            this.UserId = userId;
            this.PostId = postId;
        }
    }
}
