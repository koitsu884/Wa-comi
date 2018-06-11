using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class ClanSeekForCreationDto
    {
        [Required]
        public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        [Required]
        public int AppUserId{ get; set;}
        public string MainPhotoUrl{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public string Description{get; set;}
        [Required]
        public int LocationId{get; set;}
        public DateTime Created{get; set;} = DateTime.Now;
        public DateTime LastActive{get; set;} = DateTime.Now;
    }
}