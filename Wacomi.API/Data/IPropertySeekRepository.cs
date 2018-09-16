using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public enum PropertyRequestEnum {
        NotCare,
        Yes,
        No
    };

    public class PropertySeekParameters{
        public int? AppUserId{ get; set;}
        public int[] CategoryIds {get; set;}
        public int CityId {get; set;}
        // public PropertyTypeEnum[] PropertyTypes { get; set;}
        public RentTypeEnum[] RentTypes { get; set;}
        public PropertyRequestEnum Pet {get; set;} = PropertyRequestEnum.NotCare;
        public PropertyRequestEnum Child {get; set;} = PropertyRequestEnum.NotCare;
        public int Internet { get;set;} = 0; //0: not care, 1: limited 3:unlimited
        public GenderEnum Gender { get;set;} = GenderEnum.SECRET;
        public double? Area_top{ get; set;} = null;
        public double? Area_left{get; set;} = null;
        public double? Area_right{get; set;} = null;
        public double? Area_bottom{get; set;} = null;
        public DateTime? DateFrom{get; set;} = null;
        public int? MateMax{get; set;} = null;
        public int? MateMin{get; set;} = null;
        public int? rentMin{get; set;} = null;
        public int? rentMax{get; set;} = null;
        public TermEnum minTerm{get; set;} = TermEnum.SHORT;
        public TermEnum maxTerm{get; set;} = TermEnum.LONG;
    }
    public interface IPropertySeekRepository : IRepositoryBase
    {
        Task<Property> GetProperty(int id);
        Task<bool> PropertyExists(int id);
        Task<PagedList<Property>> GetProperties(PaginationParams paginationParams, PropertySeekParameters searchOptions);
        Task<IEnumerable<Property>> GetLatestProperties(GenderEnum? gender = null);
        Task<IEnumerable<Property>> GetPropertiesByUser(int userId);
        Task<IEnumerable<PropertySeekCategory>> GetPropertyCategories();
    }
}