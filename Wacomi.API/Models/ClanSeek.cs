using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public class ClanSeek : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id { get; set;}
        [Required]
        public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        public virtual ClanSeekCategory Category{get; set;}
        public virtual AppUser AppUser{ get; set;}
        [Required]
        public int? AppUserId { get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto{ get; set;}
        public int? MainPhotoId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;} = true;
        public string Description{get; set;}
        public int LocationId{ get; set;}
        public virtual City Location{get; set;}
        public DateTime DateCreated{get; set;} = DateTime.Now;
        public DateTime DateUpdated{get; set;} = DateTime.Now;
    }
}