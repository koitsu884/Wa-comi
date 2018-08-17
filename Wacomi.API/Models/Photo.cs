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
        public string Description {get;set;}
        public DateTime DateAdded {get;set;} = DateTime.Now;
        public string PublicId{ get; set;}
    }
}