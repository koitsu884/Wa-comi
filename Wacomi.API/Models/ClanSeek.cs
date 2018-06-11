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
        public ClanSeekCategory Category{get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public AppUser AppUser{ get; set;}
        public ICollection<Photo> Photos { get; set; }
        public string MainPhotoUrl{ get; set;}
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