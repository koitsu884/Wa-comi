namespace Wacomi.API.Models
{
    public interface IStructuredDataItem : IDataRecord
    {
        bool IsActive { get; set; }
    }
}