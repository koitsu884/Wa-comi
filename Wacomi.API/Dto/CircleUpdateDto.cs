using System;
using System.ComponentModel.DataAnnotations;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class CircleUpdateDto
    {
        public int? Id { get; set;}
        [Required]
        [MaxLength(100)]
        public string Name{ get; set;}
        [Required]
        public int CategoryId { get; set;}
        public int? CityId{ get; set;}
        [Required]
        [MaxLength(5000)]
        public string Description{ get; set;}
        bool ApprovalRequired { get; set;} = false;
        [Required]
        public int AppUserId{ get; set;}
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}