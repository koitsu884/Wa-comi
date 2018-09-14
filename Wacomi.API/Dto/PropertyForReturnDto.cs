using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class PropertyForReturnDto
    {
         public int Id { get; set;}
        public AppUserForReturnDto AppUser{ get; set;}

        public virtual ICollection<PhotoForReturnDto> Photos { get; set; }
        public PhotoForReturnDto MainPhoto{ get; set;}
        public bool IsActive{get; set;}
        public string Title{get; set;}
        public string Description{get; set;}
        public string CityName{ get; set;}
        public string CityId{ get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;} 
        public bool HasPet{ get; set;}
        public bool HasChild{ get; set;}
        public DateTime? DateAvailable{ get; set;}
        public int Internet{ get; set;} //0:none 1: unlimited 2 : limited
        public int Gender{ get; set;} //0:both 1: male 2 : female
        public TermEnum MinTerm{ get; set;} = TermEnum.SHORT;
        public TermEnum MaxTerm{ get; set;} = TermEnum.LONG;
        public int Rent{get; set;}
        public ICollection<CategoryForReturnDto> Categories { get; set; }
        
        public DateTime DateCreated{get; set;}
        public DateTime DateUpdated{get; set;}
    }
}