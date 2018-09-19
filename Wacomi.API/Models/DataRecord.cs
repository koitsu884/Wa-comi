using System;

namespace Wacomi.API.Models
{
    public abstract class DataRecord
    {
        public int Id{ get; set;}
        public DateTime DateCreated{ get; set;} = DateTime.Now;
        public DateTime DateUpdated{ get; set;} = DateTime.Now;
    }
}