﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FireBreath.PostsMicroservice.Models.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Attachment?> AttachmentPaths { get; set; } = new List<Attachment?>();
        public int SenderId { get; set; }
        public int ReceiverId {  get; set; }

        public Message()
        {
            this.Author = string.Empty;
            this.Content = string.Empty;
            this.Timestamp = DateTime.Now;
            this.SenderId = 0;
            this.ReceiverId = 0;
        }

        public Message(string content, string author, int senderId, int receiverId)
        {
            this.Author = author;
            this.Content = content;
            this.Timestamp = DateTime.Now;
            this.SenderId = senderId;
            this.ReceiverId = receiverId;
        }
    }
}
