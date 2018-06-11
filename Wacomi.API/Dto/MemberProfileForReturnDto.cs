using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class MemberProfileForReturnDto
    {
        public int Id { get; set; }
        // public string IdentityId { get; set; }
        //Private Profiles
        // public string DisplayName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        // public DateTime Created { get; set; }
        // public DateTime LastActive { get; set; }
        //Public Profiles
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public int? HomeTownId { get; set; }
        public string HomeTown { get; set; }
        public string MainPhotoUrl { get; set; }
    }
}