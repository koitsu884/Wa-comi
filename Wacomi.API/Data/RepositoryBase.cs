using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public abstract class RepositoryBase
    {
        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<T> Get<T>(int recordId) where T : DataRecord
        {
            return await _context.FindAsync<T>(recordId);
        }

        public IQueryable<T> GetDataRecordByTableName<T>(string tableName) where T : class
        {
            PropertyInfo entityProperty = _context.GetType().GetProperty(tableName);
            return (IQueryable<T>)entityProperty.GetValue(_context, null);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteAll<T>(T entities) where T : class
        {
            _context.RemoveRange(entities);
        }

        public async Task<int> SaveAll()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> RecordExist(string recordType, int id)
        {
            switch (recordType.ToLower())
            {
                case "appuser":
                    return await _context.AppUsers.AnyAsync(r => r.Id == id);
                case "businessprofile":
                    return await _context.BusinessProfiles.AnyAsync(r => r.Id == id);
                case "memberprofile":
                    return await _context.MemberProfiles.AnyAsync(r => r.Id == id);
                case "clanseek":
                    return await _context.ClanSeeks.AnyAsync(r => r.Id == id);
                case "dailytopic":
                    return await _context.DailyTopics.AnyAsync(r => r.Id == id);
                case "blog":
                    return await _context.Blogs.AnyAsync(r => r.Id == id);
                case "blogfeed":
                    return await _context.BlogFeeds.AnyAsync(r => r.Id == id);
                case "topiccomment":
                    return await _context.TopicComments.AnyAsync(r => r.Id == id);
                case "photo":
                    return await _context.Photos.AnyAsync(r => r.Id == id);

            }
            return false;
        }

        // public async Task<int> SaveAll()
        // {
        //     return await _context.SaveChangesAsync();
        // }
    }
}