using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Shares")]
    public class Share
    {
        [Key]
        [Column(Order = 1)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int PostId { get; set; }

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
