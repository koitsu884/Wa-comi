namespace Wacomi.API.Models
{
    public class AttractionLike
    {
        public int? AppUserId{ get; set;}
        public int? AttractionId { get; set;}
        public AppUser AppUser{ get; set;}
        public Attraction Attraction { get; set;}
    }
}