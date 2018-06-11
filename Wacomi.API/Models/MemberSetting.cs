using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class MemberSetting
    {
        [Key]
        public string AppUserId{ get; set;}
        public AppUser AppUser{get; set;}
        public bool AllowFriendSearch{get; set;}
        //Maybe need black list
        public bool AllowCancidateSearch{get; set;}
        public bool ShowGender{ get; set;}
        public bool ShowDOB {get; set;}
        public bool ShowHomeTown{get; set;}

    }
}