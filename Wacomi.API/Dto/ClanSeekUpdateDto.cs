using System;

namespace Wacomi.API.Dto
{
    public class ClanSeekUpdateDto
    {
        public int Id{ get; set;}
        public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        public int? MainPhotoId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public string Description{get; set;}
        public int LocationId {get; set;}
        public DateTime LastActive{get; set;} = DateTime.Now;
        public bool IsActive = true;
    }
}