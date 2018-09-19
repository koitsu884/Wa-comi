using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IRepositoryBase
    {
         void Add<T>(T entity) where T: class;
         Task<T> Get<T>(int recordId) where T : DataRecord;
         IQueryable<T> GetDataRecordByTableName<T>(string tableName) where T : class;
         void Delete<T>(T entity) where T: class;
         void DeleteAll<T>(T entities) where T: class;
         Task<int> SaveAll();
         Task<bool> RecordExist(string recordType, int id);
    }
}