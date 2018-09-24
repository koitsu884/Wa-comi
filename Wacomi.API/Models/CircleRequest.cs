using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class CircleRequest
    {
        public AppUser AppUser{ get; set;}
        [Required]
        public int AppUserId { get; set; }
        public Circle Circle {get; set;}
        [Required]
        public int CircleId{ get; set;}
        public bool Declined{ get; set;} = false;
        [MaxLength(1000)]
        public string Message { get; set;}
    }
}