using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Wacomi.API.Models
{
    public class AppUser
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

        public DateTime DateCreated {get; set;} = DateTime.Now;
        public DateTime LastActive{get; set;} = DateTime.Now;
        public virtual ICollection<Message> MessageReceived{ get; set;}
        public virtual ICollection<Message> MessageSent{ get; set;}
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Blog>  Blogs { get; set;}
        public virtual ICollection<TopicCommentFeel> TopicCommentFeels { get; set;}
    }
}