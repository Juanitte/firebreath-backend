namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AttachmentDto?> AttachmentPaths { get; set; } = new List<AttachmentDto?>();
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public MessageDto()
        {
            this.Id = 0;
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.Timestamp = DateTime.Now;
            this.SenderId = 0;
            this.ReceiverId = 0;
        }
    }
}
