using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.UsersMicroservice.Models.Entities
{
    [Table("Blocks")]
    [PrimaryKey(nameof(UserId), nameof(BlockedUserId))]
    public class Block
    {
        public int UserId { get; set; }
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
