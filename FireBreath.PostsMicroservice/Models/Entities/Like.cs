using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Likes")]
    [PrimaryKey(nameof(UserId), nameof(PostId))]
    public class Like
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Like()
        {
            this.UserId = 0;
            this.PostId = 0;
        }
        public Like(int userId, int postId)
        {
            this.UserId = userId;
            this.PostId = postId;
        }
        public Like(int userId, int postId, DateTime timestamp)
        {
            this.UserId = userId;
            this.PostId = postId;
            this.Timestamp = timestamp;
        }
    }
}
