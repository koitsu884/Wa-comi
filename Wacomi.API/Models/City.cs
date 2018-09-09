namespace Wacomi.API.Models
{
    public class City
    {
        public int Id{get; set;}
        public string Region{get; set;}
        public string Name { get; set;}
        public double? Latitude{ get; set;}
        public double? Longitude{ get; set;}
    }
}