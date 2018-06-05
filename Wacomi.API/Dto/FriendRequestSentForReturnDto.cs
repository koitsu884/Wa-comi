namespace Wacomi.API.Dto
{
    public class FriendRequestSentForReturnDto
    {
        public int RecipientId{ get; set;}
        public string RecipientDisplayName { get; set;}
        public string RecipientPhotoUrl{ get; set;}
        string Message { get; set;}
        bool Declined{ get; set;}
        bool IsRead{ get; set;}
    }
}