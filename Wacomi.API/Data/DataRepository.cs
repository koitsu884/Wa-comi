using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class DataRepository : RepositoryBase, IDataRepository
    {
        public DataRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<IEnumerable<HomeTown>> GetHomeTowns()
        {
            return await _context.HomeTowns.ToListAsync();
        }

        public async Task<PropertySeek> GetPropertySeek(int id)
        {
            return await _context.PropertySeeks.FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<PropertySeek>> GetPropertySeeks(int? categoryId = null)
        {
            if (categoryId == null)
                return await _context.PropertySeeks.ToListAsync();
            return await _context.PropertySeeks.Where(ps => ps.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<PropertySeekCategory>> GetPropertySeekCategories()
        {
            return await _context.PropertySeekCategories.ToListAsync();
        }

        public async Task<Friend> GetFriend(int memberId, int friendId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.AppUser)
                                         .FirstOrDefaultAsync(f => f.MemberId == memberId && f.FriendMemberid == friendId);
        }

        public async Task<IEnumerable<Friend>> GetFriends(int memberId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.AppUser)
                                         .Where(f => f.MemberId == memberId)
                                         .ToListAsync();
        }

        public async Task<FriendRequest> GetFriendRequest(int senderId, int recipientId)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.RecipientId == recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int memberId)
        {
            return await _context.FriendRequests.Include(fr => fr.Sender)
                                                .Include(fr => fr.Sender.AppUser)
                                                .Where(fr => fr.RecipientId == memberId)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId)
        {
            return await _context.FriendRequests.Include(fr => fr.Recipient)
                                                .Include(fr => fr.Recipient.AppUser)
                                                .Where(fr => fr.SenderId == memberId)
                                                .ToListAsync();
        }

        public async Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.RecipientId == memberId && fr.SenderId == senderId);
        }

    }
}