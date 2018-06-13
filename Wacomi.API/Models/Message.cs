using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Message
    {
        public int Id { get; set;}
        [Required]
        public int SenderId { get; set;}
        public AppUser Sender { get; set;}
        [Required]
        public int RecipientId { get; set;}
        public AppUser Recipient { get; set;}
        [MaxLength(100)]
        public string Title{ get; set;}
        [Required]
        [MaxLength(1000)]
        public string Content { get; set;}
        public bool IsRead { get; set;} = false;
        public DateTime? DateRead { get; set;}
        public DateTime DateCreated { get; set;} = DateTime.Now;
    }
}