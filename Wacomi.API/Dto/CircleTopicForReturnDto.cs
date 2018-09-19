namespace Wacomi.API.Dto
{
    public class CircleTopicForReturnDto
    {
        public int Id{ get; set;}
        public CircleForReturnDto Circle{ get; set;}
        public string Title{ get; set;}
        public string Description{ get; set;}
        public PhotoForReturnDto Photo { get; set;}
        public int CommentCount{ get; set;}
    }
}