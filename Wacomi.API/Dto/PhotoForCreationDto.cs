using System;
using Microsoft.AspNetCore.Http;

namespace Wacomi.API.Dto
{
    public class PhotoForCreationDto
    {
        public IFormFile File { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string PublicId { get; set; }
    }
}