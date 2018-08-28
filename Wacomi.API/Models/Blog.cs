using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Blog
    {
        public int Id { get; set;}

        public bool HideOwner{ get; set;} = false;
        [Required]
        public int OwnerId { get; set;}
        public AppUser Owner{ get; set;}
        public string WriterName{ get; set;}
        public string WriterIntroduction {get; set;}
        [Required]
        public string Title { get; set;}
        public string Description{get; set;}
        public string Category{get; set;}
        public string Category2{get; set;}
        public string Category3{get; set;}
        [Required]
        public string Url{get; set;}
        public Photo Photo {get; set;}
        public int? PhotoId{ get; set;}
        public string RSS{get; set;}
        public DateTime? DateRssRead{ get; set;} = DateTime.Now;
        public int FollowerCount{get; set;}
        public int HatedCount{ get; set;}
        public bool IsActive{ get; set;} = true;

    }
}