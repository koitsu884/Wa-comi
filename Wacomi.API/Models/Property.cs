using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Data;

namespace Wacomi.API.Models
{
    public enum TermEnum{
        SHORT,
        WEEK,
        WEEK_2,
        WEEK_3,
        MONTH,
        MONTH_2,
        MONTH_3,
        LONG
    };

    public enum PropertyTypeEnum{
        HOUSE,
        APPARTMENT,
        UNIT
    };

    public enum RentTypeEnum {
        OWN,
        SHARE,
        WHOLE,
        HOMESTAY
    }

    public class Property : IDataItemWithMultiplePhotos, IAppUserLinkable
    {
        public int Id { get; set;}
        [Required]
        public int? AppUserId{ get; set;}
        public AppUser AppUser{ get; set;}

        public virtual ICollection<Photo> Photos { get; set; }
        public Photo MainPhoto{ get; set;}
        public int? MainPhotoId{ get; set;}
        public bool IsActive{get; set;} = true;
        [Required]
        [MaxLength(100)]
        public string Title{ get; set;}
        [MaxLength(5000)]
        public string Description{get; set;}
        [Required]
        public int CityId{ get; set;}
        public City City{get; set;}
        // public PropertyTypeEnum PropertyType{ get; set;}
        public RentTypeEnum RentType { get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;} 
        public bool HasPet{ get; set;}
        public bool HasChild{ get; set;}
        public DateTime? DateAvailable{ get; set;}
        public int Internet{ get; set;} = 0; //0:none 1: unlimited 2 : limited
        public GenderEnum Gender{ get; set;} = GenderEnum.SECRET; //0:both 1:male 2:female
        public TermEnum MinTerm{ get; set;} = TermEnum.SHORT;
        public TermEnum MaxTerm{ get; set;} = TermEnum.LONG;
        [Required]
        public int Rent{get; set;}
        public virtual ICollection<PropertyCategorization> Categorizations {get; set;}
        
        public DateTime DateCreated{get; set;} = DateTime.Now;
        public DateTime DateUpdated{get; set;} = DateTime.Now;
    }
}