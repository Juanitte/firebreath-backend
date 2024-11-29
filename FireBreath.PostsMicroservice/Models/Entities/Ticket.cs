﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Common.Attributes.ModelAttributes;
using Common.Utilities;
using EasyWeb.TicketsMicroservice.Models.Dtos.EntityDto;

namespace EasyWeb.TicketsMicroservice.Models.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Filters]
        public string Title { get; set; }

        [Filters]
        public string Name { get; set; }

        [Filters]
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }
        public int? UserId { get; set; }
        public Priorities Priority { get; set; }
        public Status Status { get; set; }

        public bool IsAssigned { get; set; }
        public bool HasNewMessages { get; set; }
        public int NewMessagesCount { get; set; }
        public List<Message?> Messages { get; set; } = new List<Message?>();

        public Ticket()
        {
            this.Title = string.Empty;
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Timestamp = DateTime.Now;
            this.UserId = -1;
            this.Priority = Priorities.NOT_SURE;
            this.Status = Status.PENDING;
            this.IsAssigned = false;
            this.HasNewMessages = true;
            this.NewMessagesCount = 0;
        }
        public Ticket(string title, string name, string email)
        {
            this.Title = title;
            this.Name = name;
            this.Email = email;
            this.Timestamp = DateTime.Now;
            this.UserId = -1;
            this.Priority = Priorities.NOT_SURE;
            this.Status = Status.PENDING;
            this.IsAssigned = false;
            this.HasNewMessages = true;
            this.NewMessagesCount = 0;
        }

        public Ticket(Message message)
        {
            this.Title = string.Empty;
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.Timestamp = DateTime.Now;
            this.UserId = -1;
            this.Priority = Priorities.NOT_SURE;
            this.Status = Status.PENDING;
            this.Messages.Add(message);
            this.IsAssigned = false;
            this.HasNewMessages = true;
            this.NewMessagesCount = 0;
        }
        public Ticket(string title, string name, string email, Message message)
        {
            this.Title = title;
            this.Name = name;
            this.Email = email;
            this.Timestamp = DateTime.Now;
            this.UserId = -1;
            this.Priority = Priorities.NOT_SURE;
            this.Status = Status.PENDING;
            this.Messages.Add(message);
            this.IsAssigned = false;
            this.HasNewMessages = true;
            this.NewMessagesCount = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Ticket)obj;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        ///     Convierte el modelo en un objeto dto
        /// </summary>
        /// <returns></returns>
        public TicketResumeDto ToResumeDto()
        {
            return this.ConvertModel(new TicketResumeDto());
        }
    }
}
