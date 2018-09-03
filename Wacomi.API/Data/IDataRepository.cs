using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IDataRepository : IRepositoryBase
    {

         Task<IEnumerable<City>> GetCities();
         Task<IEnumerable<HomeTown>> GetHomeTowns();

        Task<PropertySeek> GetPropertySeek(int id);
        Task<IEnumerable<PropertySeek>> GetPropertySeeks(int? categoryId);
        Task<IEnumerable<PropertySeekCategory>> GetPropertySeekCategories();

        Task<Friend> GetFriend(int memberId, int friendId);
        Task<IEnumerable<Friend>> GetFriends(int memberId);
        Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId);
        Task<FriendRequest> GetFriendRequest(int senderId, int recipientId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int memberId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId);

//         Task<Message> GetMessage(int id);
//         Task<PagedList<Message>> GetReceivedMessages(PaginationParams paginationParams,int userId);
//         // IEnumerable<Message> GetLatestReceivedMessages(int userId);
// //        Task<IEnumerable<Message>> GetReceivedMessagesFrom(int userId, int senderId);
//         Task<PagedList<Message>> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId);
//         Task<PagedList<Message>> GetSentMessages(PaginationParams paginationParams, int userId);
//         // Task<IEnumerable<Message>> GetLatestSentMessages(int userId);
//         Task<PagedList<Message>> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId);
//         Task<int> GetNewMessagesCount(int userId);

    }

    
}