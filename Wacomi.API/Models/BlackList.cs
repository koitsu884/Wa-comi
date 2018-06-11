using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class BlackList
    {
        public int Id { get; set;}
        [Required]
        public int AppUserId { get; set;}
        [Required]
        public int BlockedAppUserId { get; set;}
        public bool IsSerious{ get; set;}
        public AppUser AppUser { get; set;}
        public AppUser BlockedAppUser { get; set;}
    }
}