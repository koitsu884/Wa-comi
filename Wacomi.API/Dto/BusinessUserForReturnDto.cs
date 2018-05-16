using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class BusinessUserForReturnDto
    {
        public int Id { get; set;}
        public string IdentityId { get; set;}
        //Private Profiles
        public string DisplayName { get; set;}
        public bool IsCompany{ get; set;}
        public DateTime? EstablishedDate { get; set; }
        public DateTime Created{ get; set;}
        public DateTime LastActive{ get; set;}
        //Public Profiles
        public string Introduction { get; set;}
        public int? CityId { get; set;}
        public string City { get; set;}
        public string MainPhotoUrl {get; set;}
    }
}