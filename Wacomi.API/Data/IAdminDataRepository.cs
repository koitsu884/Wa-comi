using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IAdminDataRepository : IRepositoryBase
    {
         Task<Feedback> GetFeedback(int id);
    }
}