namespace Wacomi.API.Models
{
    public class UserRecord : DataRecord
    {
        public AppUser AppUser{ get; set;}
        public int? AppUserId{ get; set;}
    }
}