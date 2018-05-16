using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Wacomi.API.Data
{
    public interface IAuthRepository
    {
        Task<AppUser> Register(AppUser user, string password);
        Task<IdentityResult>  AddRoles(AppUser user, string[] roles);
        Task<AppUser> Login(string username, string password);
        Task<AppUser> UpdateAppUser(AppUser user);
        Task<bool> AppUserExists(string username, string email);
        Task<bool> UserNameExists(string username, string exceptionId = "");
        Task<bool> EmailExists(string email, string exceptionId = "");
        Task<bool> RoleExists(string role);
        Task<IList<string>> GetRolesForAppUser(AppUser user);
        Task<IdentityResult> DelteAppUser(AppUser user);
        Task<AppUser> GetAppUser(string id);
        // Task<BusinessUser> GetBusinessUser(int id);
        // Task<Member> GetMember(int id);
        // Task<Member> GetMemberByIdentityId(string id);
        // Task<IEnumerable<Member>> GetMembers(UserParams userParams);
        // Task<IdentityResult> DeleteMember(Member member);
        // Task<bool> SaveAll();
    }
}