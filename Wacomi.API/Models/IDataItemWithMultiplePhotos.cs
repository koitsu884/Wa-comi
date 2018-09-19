using System.Collections.Generic;

namespace Wacomi.API.Models
{
    public interface IDataItemWithMultiplePhotos : IStructuredDataItem
    {
        ICollection<Photo> Photos{ get; set;}
        Photo MainPhoto { get; set;}
        int? MainPhotoId{ get; set;}
    }
}