namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string? File { get; set; }
        public bool IsVideo { get; set; }
        public string? Thumbnail { get; set; }
        public int? PostId { get; set; }
        public int? MessageId { get; set; }

        public AttachmentDto()
        {
            this.Id = 0;
            this.Path = string.Empty;
            this.PostId = 0;
            this.MessageId = 0;
            this.File = null;
            this.IsVideo = false;
            this.Thumbnail = null;
        }

    }
}
