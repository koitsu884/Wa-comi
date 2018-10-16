using System;
using System.Collections.Generic;
using Wacomi.API.Models;

namespace Wacomi.API.Dto
{
    public class BusinessProfileForReturnDto
    {
        public int Id { get; set;}
        public string AppUserId { get; set;}
        //Private Profiles
        public bool IsCompany{ get; set;}
        public DateTime? EstablishedDate { get; set; }
        //Public Profiles
        public string Introduction { get; set;}
    }
}