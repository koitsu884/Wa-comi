using System;

namespace Wacomi.API.Dto
{
    public class PhotoForReturnDto
    {
        public int Id{get;set;}
        public string Url {get;set;}
        public string ThumbnailUrl {get;set;} //local storage
        public string IconUrl {get;set;} //local storage
        public string Description {get;set;}
        public DateTime DateAdded {get;set;}
        public string PublicId{ get; set;}
    }
}