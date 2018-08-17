namespace Wacomi.API.Dto
{
    public class AppUserUpdateDto
    {
        public string DisplayName { get; set;}

        public bool IsActive{ get; set;} = true;
        public bool IsPremium{ get; set;} = false;
        //Private Profiles
        public int? MainPhotoId { get; set;}
        public int? CityId{ get; set;}
    }
}