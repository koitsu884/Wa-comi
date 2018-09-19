namespace Wacomi.API.Models
{
    public interface ISinglePhotoAttachable
    {
        Photo Photo{ get; set;}
        int PhotoId{ get; set;}
    }
}