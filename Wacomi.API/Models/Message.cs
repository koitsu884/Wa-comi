using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Message
    {
        public int Id { get; set;}
        [Required]
        public int SenderId { get; set;}
        public Member Sender { get; set;}
        [Required]
        public int RecipientId { get; set;}
        public Member Recipient { get; set;}
        [Required]
        public string Content { get; set;}
        public bool IsRead { get; set;}
        public DateTime DateRead { get; set;}
        public DateTime MessageSent { get; set;}
    }
}