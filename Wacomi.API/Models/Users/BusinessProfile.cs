using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class BusinessProfile
    {
        public int Id{get; set;}
        [Required]
        public int AppUserId { get; set;}
        public AppUser AppUser{ get; set;}
        public bool IsCompany{ get; set;}
        public DateTime? EstablishedDate { get; set; }
        //Public Profiles
        public string Introduction { get; set;}
        // public int? CityId { get; set;}
        // public City City { get; set;}
    }
}