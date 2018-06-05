namespace Wacomi.API.Dto
{
    public class FriendForReturnDto
    {
        public int Id{get; set;}
        public string Relationship{ get; set;}
        public int FriendMemberid{ get; set;}
        public string FriendDisplayName{ get; set;}
        public string FriendPhotoUrl{ get; set;}
    }
}