namespace FireBreath.UsersMicroservice.Models.Dtos.EntityDto
{
    public class FollowDto
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }

        public FollowDto()
        {
            this.UserId = 0;
            this.FollowerId = 0;
        }
        public FollowDto(int userId, int followerId)
        {
            this.UserId = userId;
            this.FollowerId = followerId;
        }
    }
}
