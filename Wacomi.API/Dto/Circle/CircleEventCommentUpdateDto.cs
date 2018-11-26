using System;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Dto.Circle
{
    public class CircleEventCommentUpdateDto
    {
        public int Id { get; set;}
        [Required]
        public int CircleEventId{ get; set;}
        [Required]
        [MaxLength(StaticData.MidTextLength)]
        public string Comment{ get; set;}
        public DateTime DateUpdated {get; set;} = DateTime.Now;
    }
}