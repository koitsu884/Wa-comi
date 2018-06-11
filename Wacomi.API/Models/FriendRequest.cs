using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class FriendRequest
    {
        public MemberProfile Sender{ get; set;}
        [Required]
        public int SenderId{ get; set;}
        public MemberProfile Recipient {get; set;}
        [Required]
        public int RecipientId{ get; set;}
        bool IsRead{ get; set;} = false;
        bool Declined{ get; set;} = false;
        string Message { get; set;}

    }
}