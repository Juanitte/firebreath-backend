using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Shares")]
    [PrimaryKey(nameof(UserId), nameof(PostId))]
    public class Share
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Share()
        {
            this.UserId = 0;
            this.PostId = 0;
        }
        public Share(int userId, int postId)
        {
            this.UserId = userId;
            this.PostId = postId;
        }
    }
}
