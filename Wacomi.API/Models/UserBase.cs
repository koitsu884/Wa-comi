using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class UserBase
    {
        public int Id { get; set;}
        [Required]
        public string IdentityId { get; set;}
        public AppUser Identity { get; set;}
        public bool IsActive{ get; set;} = true;
        public bool IsPremium{ get; set;} = false;
        //Private Profiles
        public ICollection<Photo> Photos { get; set; }
        public string MainPhotoUrl { get; set;}
        public ICollection<Blog>  Blogs { get; set;}
    }
}