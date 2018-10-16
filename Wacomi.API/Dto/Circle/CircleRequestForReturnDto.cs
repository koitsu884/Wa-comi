namespace Wacomi.API.Dto
{
    public class CircleRequestForReturnDto
    {
        public AppUserForReturnDto AppUser{ get; set;}
        public int AppUserId { get; set; }
        public CircleForReturnDto Circle {get; set;}
        public int CircleId{ get; set;}
        public bool Declined{ get; set;}
        public string Message { get; set;}
    }
}