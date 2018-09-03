using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IClanSeekRepository : IRepositoryBase
    {
        Task<ClanSeek> GetClanSeek(int id);
        Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories();
        //Task<IEnumerable<ClanSeek>> GetClanSeeks(int? categoryId, int? cityId, bool? latest);
        Task<PagedList<ClanSeek>> GetClanSeeks(PaginationParams paginationParams, int? categoryId = null, int? cityId = null);
        Task<IEnumerable<ClanSeek>> GetClanSeeksByUser(int userId);
        Task<int> GetClanSeeksCountByUser(int userId);
    }
}