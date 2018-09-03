using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IMessageRepository : IRepositoryBase
    {
          Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetReceivedMessages(PaginationParams paginationParams,int userId);
        // IEnumerable<Message> GetLatestReceivedMessages(int userId);
//        Task<IEnumerable<Message>> GetReceivedMessagesFrom(int userId, int senderId);
        Task<PagedList<Message>> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId);
        Task<PagedList<Message>> GetSentMessages(PaginationParams paginationParams, int userId);
        // Task<IEnumerable<Message>> GetLatestSentMessages(int userId);
        Task<PagedList<Message>> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId);
        Task<int> GetNewMessagesCount(int userId);
    }
}