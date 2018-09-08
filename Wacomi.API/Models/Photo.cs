using System;
using Wacomi.API.Helper;

namespace Wacomi.API.Models
{
    public enum StorageType
    {
        Local,
        Cloudinary
    } 
    public class Photo
    {
        public int Id{get;set;}
        public StorageType StorageType {get; set;}
        public string Url {get;set;}
        public string ThumbnailUrl {get;set;} //local storage
        public string IconUrl {get;set;} //local storage
        public string Description {get;set;}
        public DateTime DateAdded {get;set;} = DateTime.Now;
        public string PublicId{ get; set;}

        public string GetIconUrl(){
            string returnUrl = this.IconUrl;
            if(returnUrl == null)
                returnUrl = this.ThumbnailUrl;
            if(returnUrl == null)
                returnUrl = this.Url;

            return returnUrl;
        }

        public string GetThumbnailUrl(){
            string returnUrl = this.ThumbnailUrl;
            if(returnUrl == null)
                returnUrl = this.Url;

            return returnUrl;
        }
    }
}