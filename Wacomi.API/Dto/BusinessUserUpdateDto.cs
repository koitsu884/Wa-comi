using System;

namespace Wacomi.API.Dto
{
    public class BusinessUserUpdateDto
    {
        public DateTime? EstablishedDate { get; set; }
        //Public Profiles
        public int? CityId { get; set;}
        public string Introduction { get; set;}
        public string MainPhotoUrl{ get; set;}
    }
}