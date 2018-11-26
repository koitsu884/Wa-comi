using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto
{
    public class UserCommentUpdateDto
    {
        public int Id { get; set;}
        [Required]
        public int AppUserId{ get; set;}
        [Required]
        public int OwnerRecordId{ get; set;}
        [Required]
        [MaxLength(StaticData.MidTextLength)]
        public string Comment{ get; set;}
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}