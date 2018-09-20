using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public class Circle : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id{ get; set;}
        [Required]
        [MaxLength(100)]
        public string Name{ get; set;}
        public CircleCategory Category { get; set;}
        public AppUser AppUser { get; set; }
        public int? AppUserId { get; set; }
        [Required]
        public int CategoryId{ get; set;}
        public City City{ get; set;}
        public int? CityId{ get; set;}
        [Required]
        [MaxLength(5000)]
        public string Description{ get; set;}
        public bool ApprovalRequired { get; set;} = false;
        public virtual ICollection<CircleMember> CircleMemberList {get; set;}
        public virtual ICollection<CircleTopic> Topics{ get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto  { get; set; }
        public int? MainPhotoId  { get; set; }
        public bool IsActive  { get; set; } = true;
        public DateTime DateCreated  { get; set; } = DateTime.Now;
        public DateTime DateUpdated  { get; set; } = DateTime.Now;
    }
}