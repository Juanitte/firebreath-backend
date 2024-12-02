using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.UsersMicroservice.Models.Entities
{
    [Table("Blocks")]
    public class Block
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int BlockedUserId { get; set; }

        public Block()
        {
            this.UserId = 0;
            this.BlockedUserId = 0;
        }
        public Block(int userId, int blockedUserId)
        {
            this.UserId = userId;
            this.BlockedUserId = blockedUserId;
        }
    }
}
