using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class PropertyUpdateDto
    {
        public int Id { get; set;}
        public int AppUserId{ get ; set;}
        public int? MainPhotoId{ get; set;}
        public bool IsActive{get; set;}
        [Required]
        [MaxLength(100)]
        public string Title{get; set;}
        [MaxLength(5000)]
        public string Description{get; set;}
        [Required]
        public int CityId{ get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;} 
        public bool HasPet{ get; set;} = false;
        public bool HasChild{ get; set;} = false;
        public DateTime? DateAvailable{ get; set;}
        public int Internet{ get; set;} = 0; //0:none 1: unlimited 2 : limited
        public int Gender{ get; set;} = 0; //0:both 1: male 2 : female
        public TermEnum MinTerm{ get; set;} = TermEnum.SHORT;
        public TermEnum MaxTerm{ get; set;} = TermEnum.LONG;
        [Required]
        public int Rent{get; set;}
        public virtual ICollection<PropertyCategorization> Categorizations {get; set;}
        public DateTime DateUpdated{get; set;} = DateTime.Now;
    }
}