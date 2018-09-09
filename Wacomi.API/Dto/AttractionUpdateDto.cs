using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class AttractionUpdateDto
    {
        public int Id {get; set;}
        public int AppUserId { get; set;}
        [Required]
        public int CityId { get; set;}
        [Required]
        [MaxLength(100)]
        public string Name{ get; set;}
        [Required]
        [MaxLength(1500)]
        public string Introduction {get; set;}
        public string AccessInfo{ get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;}
        public int? Radius{ get; set;}
        //public ICollection<int> AttractionCategoryId { get; set; }
        public virtual ICollection<AttractionCategorization> Categorizations  { get; set; }
        public int? MainPhotoId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}