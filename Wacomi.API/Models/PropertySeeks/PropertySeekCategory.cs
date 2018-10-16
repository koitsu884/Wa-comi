using System.Collections.Generic;

namespace Wacomi.API.Models
{
    public class PropertySeekCategory
    {
        public int Id {get; set;}
        public string Name{ get; set;}
        public virtual ICollection<PropertyCategorization> Categorizations  { get; set; }
    }
}