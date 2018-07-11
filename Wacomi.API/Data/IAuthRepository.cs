using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Wacomi.API.Data
{
    public interface IAuthRepository
    {
        Task<Account> Register(Account user, string userType, string password);
        Task<AppUser> AddAppUser(Account account, string userType);
        Task<IdentityResult>  AddRoles(Account user, string[] roles);
        Task<Account> Login(string username, string password);
        Task<bool> EmailConfirmed(Account user);
        Task<Account> UpdateAccount(Account user);
        Task<bool> AccountExists(string username, string email);
        Task<bool> UserNameExists(string username, string exceptionId = "");
        Task<bool> EmailExists(string email, string exceptionId = "");
        Task<bool> RoleExists(string role);
        Task<IList<string>> GetRolesForAccount(Account user);
        Task<IdentityResult> DelteAccount(Account user);
        Task<Account> GetAccount(string id);
        Task<Account> GetAccountByEmail(string email);
        // Task<BusinessUser> GetBusinessUser(int id);
        // Task<Member> GetMember(int id);
        // Task<Member> GetMemberByIdentityId(string id);
        // Task<IEnumerable<Member>> GetMembers(UserParams userParams);
        // Task<IdentityResult> DeleteMember(Member member);
        // Task<bool> SaveAll();
    }
}