namespace Wacomi.API.Models
{
    public class AttractionReviewLike
    {
        public int? AppUserId{ get; set;}
        public int? AttractionReviewId { get; set;}
        public AppUser AppUser{ get; set;}
        public AttractionReview AttractionReview { get; set;}
    }
}