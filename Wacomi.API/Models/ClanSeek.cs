using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class ClanSeek
    {
        public int Id { get; set;}
        [Required]
        public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        public virtual ClanSeekCategory Category{get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public virtual AppUser AppUser{ get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public string MainPhotoUrl{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;} = true;
        public string Description{get; set;}
        public int LocationId{ get; set;}
        public virtual City Location{get; set;}
        public DateTime Created{get; set;}
        public DateTime LastActive{get; set;}
    }
}