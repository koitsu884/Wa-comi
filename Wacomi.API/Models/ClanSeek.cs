using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class ClanSeek
    {
        public int Id { get; set;}
        public string Category{get; set;}
        [Required]
        public int OwnerId{ get; set;}
        public Member Owner{ get; set;}
        public ICollection<Photo> Photos { get; set; }
        public string MainPhotoUrl{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;}
        public string Description{get; set;}
        public string Location{get; set;}
        public DateTime Created{get; set;}
        public DateTime LastActive{get; set;}
    }
}