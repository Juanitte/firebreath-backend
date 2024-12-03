using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.UsersMicroservice.Models.Entities
{
    [Table("Follows")]
    [PrimaryKey(nameof(UserId), nameof(FollowerId))]
    public class Follow
    {
        public int UserId { get; set; }
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
