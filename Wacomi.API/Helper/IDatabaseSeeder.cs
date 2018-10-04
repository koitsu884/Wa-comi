using Microsoft.AspNetCore.Identity;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public interface IDatabaseSeeder
    {
         void Seed(UserManager<Account> userManager,
            // UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context);
    }
}