using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Friend
    {
        [Required]
        public int MemberId{ get; set;}
        public Member Member{ get; set;}
        [Required]
        public int FriendMemberid{ get; set;}
        public Member FriendMember{ get; set;}
        [Required]
        public string Relationship{ get; set;}
    }
}