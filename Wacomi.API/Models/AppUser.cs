using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public class AppUser : IDataItemWithMultiplePhotos
    {
        public int Id { get; set;}
        [Required]
        public string AccountId { get; set;}
        public Account Account { get; set;}
        public string UserType {get; set;}
        public int UserProfileId { get; set;}
        public string DisplayName { get; set;}

        public bool IsActive{ get; set;} = true;
        public bool IsPremium{ get; set;} = false;
        //Private Profiles
        public Photo MainPhoto { get; set;}
        public int? MainPhotoId { get; set;}
        public int? CityId { get; set;}
        public City City{ get; set;}
        public int TotalLike {get; set;} = 0;
        public int TotalLikeM {get; set;} = 0;
        public int TotalLikeW {get; set;} = 0;

        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime DateUpdated {get; set;} = DateTime.Now;
        public DateTime LastActive{get; set;} = DateTime.Now;
        public virtual ICollection<Message> MessageReceived{ get; set;}
        public virtual ICollection<Message> MessageSent{ get; set;}
        public virtual ICollection<CircleRequest> CircleRequestsSent {get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Blog>  Blogs { get; set;}
        public virtual ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
        public virtual ICollection<CircleMember> CircleMembers { get; set;}
    }
}