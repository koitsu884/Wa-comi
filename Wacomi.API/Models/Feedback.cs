using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Feedback
    {
        public int Id { get; set;}
        public int? SenderId { get; set;}
        public AppUser Sender { get; set;}
        [Required]
        [MaxLength(100)]
        public string SenderName { get; set;}
        [Required]
        public string Email { get; set;}
        [MaxLength(100)]
        [Required]
        public string Title{ get; set;}
        [Required]
        public string Content { get; set;}
        public DateTime DateCreated { get; set;} = DateTime.Now;
    }
}