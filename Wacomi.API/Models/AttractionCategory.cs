using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class AttractionCategory
    {
        public int Id{ get; set;}
        [Required]
        public string Name{ get; set;}
        public virtual ICollection<AttractionCategorization> Categorizations  { get; set; }
    }
}