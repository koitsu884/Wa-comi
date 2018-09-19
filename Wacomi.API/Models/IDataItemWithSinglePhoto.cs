namespace Wacomi.API.Models
{
    public interface IDataItemWithSinglePhoto : IStructuredDataItem
    {
        Photo Photo{ get; set;}
        int? PhotoId{ get; set;}
    }
}