using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class AdminDataRepository : RepositoryBase, IAdminDataRepository
    {
        public AdminDataRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Feedback> GetFeedback(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == id); 
        }
    }
}