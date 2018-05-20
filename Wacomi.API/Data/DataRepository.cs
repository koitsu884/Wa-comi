using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;
        public DataRepository(ApplicationDbContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<IEnumerable<HomeTown>> GetHomeTowns()
        {
            return await _context.HomeTowns.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id)
        {
            className =className.ToLower();
            switch(className){
                case "member":
                    var member = await _context.Members.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if(member != null){
                        return member.Photos.ToList();
                    }
                    break;
                case "business":
                    var business = await _context.BusinessUsers.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if(business != null){
                        return business.Photos.ToList();
                    }
                    break;
                case "clanseek":
                    var clanSeek = await _context.ClanSeeks.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if(clanSeek != null){
                        return clanSeek.Photos.ToList();
                    }
                    break;
            }
            return null;
        }
        public async Task<UserBase> GetUser(string type, int id){
            switch(type.ToLower()){
                case "member":
                    return await GetMember(id);
                case "business":
                    return await GetBusinessUser(id);
                default:
                 return null;
            }
        }

        public async Task<BusinessUser> GetBusinessUser(int id){
            return await _context.BusinessUsers.Include(m => m.City)
                                        .Include(m=>m.Identity)
                                        .Include(m=>m.Photos)
                                        .Include(m=>m.Blogs)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Member> GetMember(int id){
            return await _context.Members.Include(m => m.City)
                                        .Include(m => m.HomeTown)
                                        .Include(m=>m.Identity)
                                        .Include(m=>m.Photos)
                                        .Include(m=>m.Blogs)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Member> GetMemberByIdentityId(string id){
            return await _context.Members.Include(m => m.City)
                                        .Include(m => m.HomeTown)
                                        .Include(m => m.Identity)
                                        .Include(m=>m.Photos)
                                        .Include(m=>m.Blogs)
                                        .FirstOrDefaultAsync(m => m.IdentityId == id);
        }
        public async Task<IEnumerable<Member>> GetMembers(UserParams userParams){
            return await _context.Members.Where(m => m.IsActive == true).ToListAsync();
        }

        public async Task<Blog> GetBlog(int id){
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id)
        {
            className =className.ToLower();
            switch(className){
                case "member":
                    var member = await _context.Members.Include(m => m.Blogs).FirstOrDefaultAsync(m => m.Id == id);
                    if(member != null){
                        return member.Blogs.ToList();
                    }
                    break;
                case "business":
                    var business = await _context.BusinessUsers.Include(b => b.Blogs).FirstOrDefaultAsync(b => b.Id == id);
                    if(business != null){
                        return business.Blogs.ToList();
                    }
                    break;
            }
            return null;
        }

        public async Task<IEnumerable<Blog>> GetBlogs(){
            return await _context.Blogs.Where(b => b.IsActive == true).ToListAsync();
        }

        public async Task<BlogFeed> GetLatestBlogFeed(Blog blog){
            return await _context.BlogFeeds.Where(bf => bf.BlogId == blog.Id).OrderByDescending(bf => bf.Id).FirstOrDefaultAsync();
        }

        public async Task<BlogFeed> GetBlogFeed(int id){
            return await _context.BlogFeeds.FirstOrDefaultAsync(bf => bf.Id == id);
        }

        public async Task<IEnumerable<BlogFeed>> GetLatestBlogFeeds(){
            return await _context.BlogFeeds.Include(bf => bf.Blog)
                                            .Where(bf => bf.Blog.IsActive == true)
                                            .OrderByDescending(bf => bf.PublishingDate)
                                            .Take(50).ToListAsync();
        }
    }
}