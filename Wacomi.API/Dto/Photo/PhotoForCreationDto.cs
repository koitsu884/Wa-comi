using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class PhotoForCreationDto
    {
        [Required]
        public List<IFormFile> File { get; set;}
        public StorageType StorageType{ get; set;}
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string PublicId { get; set; }
    }
}