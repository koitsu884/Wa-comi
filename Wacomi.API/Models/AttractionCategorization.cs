using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wacomi.API.Models
{
    public class AttractionCategorization
    {
        public virtual Attraction Attraction { get; set;}
        public int AttractionId{ get; set;}
        public virtual AttractionCategory AttractionCategory { get; set;}
        public int AttractionCategoryId { get; set;}
    }
}