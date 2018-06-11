using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public enum BlogPreferenceEnum{
        Follow,
        Ignore,
        Hate,
        Default
    }
    public class BlogPreference
    {
        public int Id { get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public AppUser AppUser{ get; set;}
        [Required]
        public int BlogId{ get; set;}
        public Blog Blog{ get; set;}
        public BlogPreferenceEnum Preference { get; set;}
    }
}