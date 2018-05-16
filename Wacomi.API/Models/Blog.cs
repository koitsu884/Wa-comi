using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class Blog
    {
        public int Id { get; set;}

        public bool HideOwner{ get; set;} = false;
        public string WriterName{ get; set;}
        public string WriterIntroduction {get; set;}
        public string Title { get; set;}
        public string Description{get; set;}
        public string Category{get; set;}
        public string Category2{get; set;}
        public string Category3{get; set;}
        public string Url{get; set;}
        public string RSS{get; set;}
        public int FollowerCount{get; set;}
        public int HatedCount{ get; set;}
        public bool IsActive{ get; set;}

    }
}