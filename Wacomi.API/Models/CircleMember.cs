namespace Wacomi.API.Models
{
    public enum CircleRoleEnum {
        OWNER,
        MEMBER
    }
    public class CircleMember
    {
        public Circle Circle{get; set;}
        public int CircleId{ get; set;}
        public AppUser AppUser { get; set;}
        public int AppUserId { get; set;}
        public CircleRoleEnum Role { get; set;}
    }
}