using System;
using Microsoft.AspNetCore.Identity;

namespace Wacomi.API.Models
{
    public class AppUser : IdentityUser
    {
        public string UserType {get; set;}
        public int RelatedUserClassId{ get; set;}
        public string DisplayName { get; set;}
        public DateTime Created {get; set;} = DateTime.Now;
        public DateTime LastActive{get; set;} = DateTime.Now;
    }
}