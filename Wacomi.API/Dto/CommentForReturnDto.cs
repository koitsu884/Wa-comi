namespace Wacomi.API.Dto
{
    public class CommentForReturnDto
    {
        public int Id{ get; set;}
        public string OwnerRecordClass{ get; set;}
        public int OwnerRecordId { get; set;}
        public int AppUserId { get; set;}
        public string DisplayName{ get; set;}
        public string IconUrl{ get; set;}
        public string Comment{ get; set;}
        public System.DateTime DateCreated { get; set;} = System.DateTime.Now;
    }
}