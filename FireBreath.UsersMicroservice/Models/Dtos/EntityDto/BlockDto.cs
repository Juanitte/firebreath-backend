namespace FireBreath.UsersMicroservice.Models.Dtos.EntityDto
{
    public class BlockDto
    {
        public int UserId { get; set; }
        public int BlockedUserId { get; set; }

        public BlockDto()
        {
            this.UserId = 0;
            this.BlockedUserId = 0;
        }
        public BlockDto(int userId, int blockedUserId)
        {
            this.UserId = userId;
            this.BlockedUserId = blockedUserId;
        }
    }
}
