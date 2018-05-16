using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Wacomi.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        public AuthRepository(ApplicationDbContext context,
                             UserManager<AppUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<AppUser> Login(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        userToVerify.LastActive = DateTime.Now;
                        await _context.SaveChangesAsync();
                        return userToVerify;
                    }
                }
            }
            // Credentials are invalid, or account doesn't exist
            return null;
        }

        public async Task<AppUser> Register(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if(!result.Succeeded){
                string errors = "";
                foreach(var error in result.Errors){
                    errors += error.Description + ", ";
                }
                throw new Exception(errors);
            }

            int relatedUserId = 0;
            switch(user.UserType){
                case "Member":
                    var newMbmer = new Member(){
                        IdentityId = user.Id
                    };

                    _context.Add(newMbmer);
                    if( await _context.SaveChangesAsync() > 0)
                    {
                            relatedUserId = newMbmer.Id;
                    }
                    break;
                case "Business": 
                    var businessUser = new BusinessUser(){
                        IdentityId = user.Id
                    };
                    
                    _context.Add(businessUser);
                    if( await _context.SaveChangesAsync() > 0)
                    {
                        relatedUserId = businessUser.Id;
                    }
                    break;
                default:
                    return user;
            }

            if(relatedUserId > 0)
            {
                user.RelatedUserClassId = relatedUserId;
                result = await _userManager.UpdateAsync(user); 
                if(result.Succeeded){
                    return user;
                }
            }            

            throw new Exception("Failed to save register infromation");
        }



        public async Task<bool> AppUserExists(string username, string email)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == username || x.Email == email))
                return true;
            return false;
        }

        public async Task<bool> UserNameExists(string username, string exceptionId = "")
        {
            if(await _context.Users.Where(x=>x.Id != exceptionId).AnyAsync(x => x.UserName == username))
                return true;
            return false;
        }

        public async Task<bool> EmailExists(string email, string exceptionId = "")
        {
            if(await _context.Users.Where(x=>x.Id != exceptionId).AnyAsync(x => x.Email == email))
                return true;
            return false;
        }

        public async Task<bool> RoleExists(string role){
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<AppUser> GetAppUser(string id){
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<AppUser> UpdateAppUser(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if( !result.Succeeded){
                string errorString = "";
                foreach (var error in result.Errors)
                {
                    errorString += error + "\n";
                }
                throw new Exception(errorString);
            }
            return user;
        }

        public async Task<IList<string>> GetRolesForAppUser(AppUser user){
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //  public async Task<BusinessUser> GetBusinessUser(int id){
        //     return await _context.BusinessUsers.Include(m => m.City)
        //                                 .Include(m=>m.Identity)
        //                                 .FirstOrDefaultAsync(m => m.Id == id);
        // }
        // public async Task<Member> GetMember(int id){
        //     return await _context.Members.Include(m => m.City)
        //                                 .Include(m => m.HomeTown)
        //                                 .Include(m=>m.Identity)
        //                                 .FirstOrDefaultAsync(m => m.Id == id);
        // }
        // public async Task<Member> GetMemberByIdentityId(string id){
        //     return await _context.Members.Include(m => m.City)
        //                                 .Include(m => m.HomeTown)
        //                                 .Include(m => m.Identity)
        //                                 .FirstOrDefaultAsync(m => m.IdentityId == id);
        // }
        // public async Task<IEnumerable<Member>> GetMembers(UserParams userParams){
        //     return await _context.Members.Where(m => m.IsActive == true).ToListAsync();
        // }
        public async Task<IdentityResult> AddRoles(AppUser user, string[] roles)
        {
            await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
            return await this._userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> DelteAppUser(AppUser user)
        {
            return await this._userManager.DeleteAsync(user);
        }

        // public async Task<IdentityResult> DeleteMember(Member member){
        //     var appUser = await this.GetAppUser(member.IdentityId);
        //     return await this._userManager.DeleteAsync(appUser); //Member will be deleted (Cascade delete)
        // }
    }
}