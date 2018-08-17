using System;

namespace Wacomi.API.Dto
{
    public class PropertySeekUpdateDto
    {
        public int? CategoryId{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;}
        public string Description{get; set;}
        public int LocationId{ get; set;}
        public DateTime LastActive{get; set;} = DateTime.Now;
    }
}