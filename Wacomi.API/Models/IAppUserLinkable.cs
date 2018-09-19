namespace Wacomi.API.Models
{
    public interface IAppUserLinkable
    {
        AppUser AppUser { get; set; }
        int? AppUserId { get; set; }
    }
}