using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class MemberProfileUpdateDto
    {
        public string Gender { get; set;}
        public DateTime? DateOfBirth { get; set; }
        //Public Profiles
        public int? CityId { get; set;}
        public int? HomeTownId {get; set;}
        public string Introduction { get; set;}
        public string Interests{ get; set;}
    }
}