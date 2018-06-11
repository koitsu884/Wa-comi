using System;

namespace Wacomi.API.Dto
{
    public class MessageForReturnDto
    {
        public int Id { get; set;}
        public string SenderId { get; set;}
        public string SenderDisplayName{ get; set;}
        public string SenderPhotoUrl{ get; set;}
        public string RecipientId { get; set;}
        public string RecipientDisplayName{ get; set;}
        public string RecipientPhotoUrl{ get; set;}
        public string Content { get; set;}
        public bool IsRead { get; set;}
        public DateTime DateRead { get; set;}
         public DateTime DateCreated { get; set;}
    }
}