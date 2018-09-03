using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class MessageRepository : RepositoryBase, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.Include(m => m.Recipient)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Include(m => m.Sender)
                                          .ThenInclude(u => u.MainPhoto)
                                          .FirstOrDefaultAsync(m => m.Id == id);
        }

        // public IEnumerable<Message> GetLatestReceivedMessages(int userId)
        // {
        //     var queryGroup = _context.Messages.Include(m => m.Recipient)
        //                                   .Include(m => m.Sender)
        //                                   .Where(m => m.RecipientId == userId)
        //                                   .GroupBy(m => m.SenderId).ToList();

        //     return queryGroup.Select(g => g.OrderByDescending(m => m.DateCreated).First()).ToList();

        //     // return await _context.Messages.Where(m => m.RecipientId == userId)
        //     //                               .Include(m => m.Recipient)
        //     //                               .Include(m => m.Sender)
        //     //                               .GroupBy(m => m.SenderId)
        //     //                           //    .Select(g => g.OrderByDescending(m => m.DateCreated).First())
        //     //                               .Select(m => m.First())
        //     //                               .ToListAsync();
        // }

        public async Task<PagedList<Message>> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                          .Include(m => m.Sender)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.RecipientId == userId && m.SenderId == senderId)
                                          .OrderByDescending(m => m.DateCreated);

            return await PagedList<Message>.CreateAsync(messages, paginationParams.PageNumber, paginationParams.PageSize);

        }

        // public async Task<IEnumerable<Message>> GetLatestSentMessages(int userId)
        // {
        //     return await _context.Messages.Include(m => m.Recipient)
        //                                   .Include(m => m.Sender)
        //                                   .Where(m => m.SenderId == userId)
        //                                   .GroupBy(m => m.RecipientId)
        //                                   .Select(g => g.OrderByDescending(m => m.DateCreated).First())
        //                                   .ToListAsync();
        // }

        public async Task<PagedList<Message>> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Include(m => m.Sender)
                                          .Where(m => m.SenderId == userId && m.RecipientId == recipientId)
                                          .OrderByDescending(m => m.DateCreated);
            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<Message>> GetReceivedMessages(PaginationParams paginationParams, int userId)
        {
            var messages = _context.Messages.Include(m => m.Sender)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.RecipientId == userId)
                                          .OrderByDescending(m => m.DateCreated);
            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<Message>> GetSentMessages(PaginationParams paginationParams, int userId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.SenderId == userId)
                                          .OrderByDescending(m => m.DateCreated);

            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<int> GetNewMessagesCount(int userId)
        {
            return await _context.Messages.Where(m => m.RecipientId == userId && m.IsRead == false).CountAsync();
        }
    }
}