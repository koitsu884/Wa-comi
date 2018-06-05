namespace Wacomi.API.Dto
{
    public class FriendRequestReceivedForReturnDto
    {
        public int SenderId{ get; set;}
        public string SenderDisplayName { get; set;}
        public string SenderPhotoUrl{ get; set;}
        string Message { get; set;}
        bool Declined{ get; set;}
        bool IsRead{ get; set;}
    }
}