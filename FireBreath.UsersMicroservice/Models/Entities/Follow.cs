using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.UsersMicroservice.Models.Entities
{
    [Table("Follows")]
    public class Follow
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int FollowerId { get; set; }

        public Follow()
        {
            this.UserId = 0;
            this.FollowerId = 0;
        }
        public Follow(int userId, int followerId)
        {
            this.UserId = userId;
            this.FollowerId = followerId;
        }
    }
}
