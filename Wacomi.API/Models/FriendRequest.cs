using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class FriendRequest
    {
        public Member Sender{ get; set;}
        [Required]
        public int SenderId{ get; set;}
        public Member Recipient {get; set;}
        [Required]
        public int RecipientId{ get; set;}
        bool IsRead{ get; set;}
        bool IsSecret{ get; set;}
        string Message { get; set;}

    }
}