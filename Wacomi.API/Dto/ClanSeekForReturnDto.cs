using System;

namespace Wacomi.API.Dto
{
    public class ClanSeekForReturnDto
    {
        public int Id { get; set;}
        public string Title{ get; set;}
        public int? CategoryId{ get; set;}
        public string CategoryName{get; set;}
        public int AppUserId{ get; set;}
        public string DisplayName{ get; set;}
        public string MainPhotoUrl{ get; set;}
        public string WebsiteUrl{ get; set;}
        public string Email { get; set;}
        public bool IsActive{get; set;}
        public string Description{get; set;}
        public int LocationId{ get; set;}
        public string LocationName{get; set;}
        public DateTime Created{get; set;}
        public DateTime LastActive{get; set;}
    }
}