namespace Wacomi.API.Models
{
    public class PropertyCategorization
    {
        public virtual Property Property { get; set;}
        public int PropertyId{ get; set;}
        public virtual PropertySeekCategory PropertySeekCategory { get; set;}
        public int PropertySeekCategoryId { get; set;}
    }
}