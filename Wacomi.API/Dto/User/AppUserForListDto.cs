using System;

namespace Wacomi.API.Dto
{
    public class AppUserForListDto
    {
        public int Id { get; set;}
        public string DisplayName { get; set;}

        public bool IsActive{ get; set;}
        public bool IsPremium{ get; set;}
        public PhotoForReturnDto MainPhoto { get; set;}
        public string CityName { get; set;}

        public DateTime DateCreated {get; set;}
        public DateTime LastActive{get; set;}
    }
}