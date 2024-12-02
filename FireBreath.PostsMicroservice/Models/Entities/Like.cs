using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Likes")]
    public class Like
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int PostId { get; set; }

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
    }
}
