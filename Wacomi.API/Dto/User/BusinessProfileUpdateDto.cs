using System;

namespace Wacomi.API.Dto
{
    public class BusinessProfileUpdateDto
    {
        public DateTime? EstablishedDate { get; set; }
        //Public Profiles
        public int? CityId { get; set;}
        public string Introduction { get; set;}
        public bool IsCompany{ get; set;}
    }
}