using System.Threading.Tasks;

namespace Wacomi.API.Data
{
    public interface IRepositoryBase
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         void DeleteAll<T>(T entities) where T: class;
         Task<int> SaveAll();
         Task<bool> RecordExist(string recordType, int id);
    }
}