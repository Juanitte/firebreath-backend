namespace FireBreath.PostsMicroservice.Models.Dtos.CreateDto
{
    public class CreateMessageDto
    {
        public string Content { get; set; }
        public List<IFormFile?> Attachments { get; set; } = new List<IFormFile?>();
        public int SenderId { get; set; }
        public int ChatId { get; set; }

        public CreateMessageDto()
        {
            this.Content = string.Empty;
            this.SenderId = 0;
            this.ChatId = 0;
        }
        public CreateMessageDto(string content, int chatId, int senderId)
        {
            this.Content = content;
            this.SenderId = senderId;
            this.ChatId = chatId;
        }
        public CreateMessageDto(string content, List<IFormFile?> attachments, int chatId, int senderId)
        {
            this.Content = content;
            this.SenderId = senderId;
            this.ChatId = chatId;
            this.Attachments = attachments;
        }
    }
}
