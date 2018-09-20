using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class CircleRepository : RepositoryBase, ICircleRepository
    {
        public CircleRepository(ApplicationDbContext context) : base(context) { }

        public void Test(){
            Dictionary<string, string> typeNameMap = new Dictionary<string, string>();
            var dbSetList = _context.GetType().GetProperties();
            foreach(var dbSet in dbSetList){
                typeNameMap.Add(dbSet.GetType().ToString(), dbSet.Name);
            }
        }

        public async Task<bool> CircleExists(int id)
        {
            return await _context.Circles.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CheckUpdatePermission(int userId){
            return await _context.CircleMembers.AnyAsync(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.OWNER);
        }
        public async Task<Circle> GetCircle(int id)
        {
            return await _context.Circles.Include(c => c.Category)
                                         .Include(c => c.City)
                                         .Include(c => c.AppUser)
                                         .Include(c => c.Photos)
                                         .Include(c => c.MainPhoto)
                                         .Include(c => c.CircleMemberList)
                                        .Include(c => c.Topics)
                                        .Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CircleCategory>> GetCircleCategories()
        {
            return await _context.CircleCategories.ToListAsync();
        }

        public async Task<int> GetCircleCountForUser(int userId)
        {
            return await _context.CircleMembers.CountAsync(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.OWNER);
        }

        public async Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions)
        {
            var query = _context.Circles.Include(c => c.Category)
                                         .Include(c => c.City)
                                         .Include(c => c.MainPhoto)
                                         .Include(c => c.CircleMemberList)
                                        .AsQueryable();

            if (searchOptions.CityId > 0)
                query = query.Where(c => c.CityId == searchOptions.CityId);
            if (searchOptions.CategoryId > 0)
                query = query.Where(c => c.Category.Id == searchOptions.CategoryId);

            return await PagedList<Circle>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<AppUser>> GetCircleMemberList(int circleId, CircleRoleEnum? role = null)
        {
            var query = _context.CircleMembers.Include(cm => cm.AppUser)
                                            .Where(cm => cm.CircleId == circleId)
                                            // .Select(cm => cm.AppUser)
                                            .AsQueryable();
            if(role != null)
                query = query.Where( cm => cm.Role == role);
            
            return await query.Select(cm => cm.AppUser).ToListAsync();
        }
        public async Task<IEnumerable<Circle>> GetCirclesByUser(int userId)
        {
            return await _context.CircleMembers.Include(cm => cm.Circle).ThenInclude(c => c.MainPhoto)
                                            .Include(cm => cm.Circle).ThenInclude(c => c.City)
                                            .Include(cm => cm.Circle).ThenInclude(c => c.Category)
                                            .Where(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.OWNER)
                                            .Select(cm => cm.Circle)
                                            .ToListAsync();
        }

        public async Task<CircleTopic> GetCircleTopic(int id)
        {
            return await _context.CircleTopic.Include(ct => ct.AppUser)
                                             .Include(ct => ct.Circle)
                                             .Include(ct => ct.Photo)
                                             .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<PagedList<CircleTopic>> GetCircleTopicList(PaginationParams paginationParams, int circleId)
        {
            var query = _context.CircleTopic.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                 .Include(ct => ct.TopicComments)
                                 .Include(ct => ct.Photo)
                                 .Where(ct => ct.CircleId == circleId)
                                 .AsQueryable();

            return await PagedList<CircleTopic>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId)
        {
            var query = _context.CircleTopicComments.Include(tc => tc.AppUser).ThenInclude(a => a.MainPhoto)
                                                    .Include(tc => tc.Replies)
                                                    .Include(tc => tc.Photo)
                                                    .AsQueryable();

            return await PagedList<CircleTopicComment>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId)
        {
            return await _context.CircleTopicCommentReplies.Where(tr => tr.CommentId == circleTopicCommentId).ToListAsync();
        }

        public async Task<IEnumerable<Circle>> GetLatestCircles()
        {
            return await _context.Circles.OrderByDescending(c => c.DateCreated).Take(10).ToListAsync();
        }
    }
}