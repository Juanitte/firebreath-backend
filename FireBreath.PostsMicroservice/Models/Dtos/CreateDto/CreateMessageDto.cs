namespace FireBreath.PostsMicroservice.Models.Dtos.CreateDto
{
    public class CreateMessageDto
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public List<IFormFile?> Attachments { get; set; } = new List<IFormFile?>();
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }

        public CreateMessageDto()
        {
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.SenderId = 0;
            this.ReceiverId = 0;
        }
        public CreateMessageDto(string author, string content, int senderId, int receiverId)
        {
            this.Author = author;
            this.Content = content;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }
        public CreateMessageDto(string author, string content, List<IFormFile?> attachments, int senderId, int receiverId)
        {
            this.Author = author;
            this.Content = content;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
            this.Attachments = attachments;
        }
    }
}
