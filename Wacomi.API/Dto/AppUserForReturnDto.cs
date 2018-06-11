using System;

namespace Wacomi.API.Dto
{
    public class AppUserForReturnDto
    {
        public int Id { get; set;}
        public string AccountId { get; set;}
        public string UserType {get; set;}
        public int UserProfileId { get; set;}
        public string DisplayName { get; set;}

        public bool IsActive{ get; set;}
        public bool IsPremium{ get; set;}
        //Private Profiles
        public string MainPhotoUrl { get; set;}
        public int CityId { get; set;}
        public string City { get; set;}

        public DateTime DateCreated {get; set;}
        public DateTime LastActive{get; set;}
    }
}