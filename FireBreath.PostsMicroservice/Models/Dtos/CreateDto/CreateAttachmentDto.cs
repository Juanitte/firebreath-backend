namespace FireBreath.PostsMicroservice.Models.Dtos.CreateDto
{
    public class CreateAttachmentDto
    {
        public string Path { get; set; }
        public int PostId { get; set; }
        public int MessageId { get; set; }

        public CreateAttachmentDto()
        {
            this.Path = string.Empty;
            this.PostId = 0;
            this.MessageId = 0;
        }

        public CreateAttachmentDto(string path, int postId, int messageId)
        {
            this.Path = path;
            this.PostId = postId;
            this.MessageId = messageId;
        }
    }
}
