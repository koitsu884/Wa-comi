using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class BlackList
    {
        public int Id { get; set;}
        [Required]
        public int MemberId { get; set;}
        [Required]
        public int BlockedMemberId { get; set;}
        public bool IsSerious{ get; set;}
        public Member Member { get; set;}
        public Member BlockedMember { get; set;}
    }
}