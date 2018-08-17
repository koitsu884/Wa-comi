using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class PropertySeek
    {
        public int Id { get; set;}
        public int? CategoryId{ get; set;}
        public PropertySeekCategory Category{get; set;}
        [Required]
        public string OwnerAppUserId{ get; set;}
        public AppUser OwnerAppUser{ get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto{ get; set;}
        public int? MainPhotoId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;}
        public string Description{get; set;}
        public int LocationId{ get; set;}
        public City Location{get; set;}
        public DateTime Created{get; set;}
        public DateTime LastActive{get; set;}
    }
}