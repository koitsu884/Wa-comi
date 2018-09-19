using System;

namespace Wacomi.API.Models
{
    public interface IDataRecord
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}