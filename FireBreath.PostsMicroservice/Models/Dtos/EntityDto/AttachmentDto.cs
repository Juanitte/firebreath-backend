namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int PostId { get; set; }
        public int MessageId { get; set; }

        public AttachmentDto()
        {
            this.Id = 0;
            this.Path = string.Empty;
            this.PostId = 0;
            this.MessageId = 0;
        }

    }
}
