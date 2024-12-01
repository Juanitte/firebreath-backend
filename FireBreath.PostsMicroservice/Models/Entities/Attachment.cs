using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using FireBreath.PostsMicroservice.Models.Entities;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Attachments")]
    public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Path { get; set; }
        public int PostId { get; set; }
        public int MessageId { get; set; }
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        [ForeignKey("MessageId")]
        public Message? Message { get; set; }

        public Attachment()
        {
            this.Path = string.Empty;
        }

        public Attachment(string path, int postId, int messageId)
        {
            this.Path = path;
            this.PostId = postId;
            this.MessageId = messageId;
        }
    }
}
