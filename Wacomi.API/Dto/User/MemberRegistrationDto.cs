using System;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class MemberRegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        //Private Profiles
        public string DisplayName { get; set;}
        public string Gender { get; set;}
        public DateTime? DateOfBirth { get; set; }
        //Public Profiles
        public int? CityId { get; set;}
        public int? HomeTownId {get; set;}
        public bool IsActive{get; set;} = true;
    }
}