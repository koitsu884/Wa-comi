using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class BlogForUpdateDto
    {
        public bool HideOwner{ get; set;} = false;
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
        public string RSS{get; set;}
    }
}