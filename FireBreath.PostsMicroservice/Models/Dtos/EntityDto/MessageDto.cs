namespace FireBreath.PostsMicroservice.Models.Dtos.EntityDto
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LastEdited { get; set; }
        public List<AttachmentDto?> Attachments { get; set; } = new List<AttachmentDto?>();
        public int SenderId { get; set; }
        public int ChatId { get; set; }

        public MessageDto()
        {
            this.Id = 0;
            this.Content = string.Empty;
            this.Timestamp = DateTime.UtcNow;
            this.LastEdited = DateTime.UtcNow;
            this.SenderId = 0;
            this.ChatId = 0;
        }
    }
}
