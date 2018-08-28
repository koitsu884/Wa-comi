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
using Microsoft.AspNetCore.Mvc;

namespace Wacomi.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthRepository(ApplicationDbContext context,
                             UserManager<Account> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<Account> Login(string userName, string password)
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
                        // userToVerify.LastActive = DateTime.Now;
                        var appUser = await _context.AppUsers.Where(u => u.AccountId == userToVerify.Id).FirstOrDefaultAsync();
                        if (appUser != null)
                        {
                            appUser.LastActive = DateTime.Now;
                            // userToVerify.AppUser = appUser;
                        }
                        await _context.SaveChangesAsync();
                        return userToVerify;
                    }
                }
            }
            // Credentials are invalid, or account doesn't exist
            return null;
        }

         public async Task<bool> EmailConfirmed(Account user){
             return await _userManager.IsEmailConfirmedAsync(user);
         }


        public async Task<AppUser> AddAppUser(Account account, string userType)
        {
            if(await _userManager.FindByIdAsync(account.Id) == null){
                return null;
            }

            var appUser = new AppUser()
            {
                AccountId = account.Id,
                DisplayName = account.UserName,
                UserType = userType
            };

            _context.Add(appUser);


            if (await _context.SaveChangesAsync() > 0)
            {
                switch (userType)
                {
                    case "Member":
                        var newMember = new MemberProfile() { AppUserId = appUser.Id };
                        _context.Add(newMember);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            appUser.UserProfileId = newMember.Id;
                            return appUser;
                        }
                        break;
                    case "Business":
                        var newBusiness = new BusinessProfile() { AppUserId = appUser.Id };
                        _context.Add(newBusiness);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            appUser.UserProfileId = newBusiness.Id;
                            return appUser;
                        }
                        break;
                    default:
                        return appUser;
                }
            }

            return null;
        }
        public async Task<Account> Register(Account user, string userType, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                string errors = "";
                foreach (var error in result.Errors)
                {
                    errors += error.Description + ", ";
                }
                throw new Exception(errors);
            }

            // var appUser = await AddAppUser(user, userType);
            // user.AppUserId = appUser.Id;
            // result = _userManager.UpdateAsync(user).Result;

            // _context.Add(appUser);

            return user;

            // switch(userType){
            //     case "Member":
            //         var newMbmer = new MemberProfile(){
            //             AppUserId
            //         };

            //         _context.Add(newMbmer);
            //         if( await _context.SaveChangesAsync() > 0)
            //         {
            //                 relatedUserId = newMbmer.Id;
            //         }
            //         break;
            //     case "Business": 
            //         var businessUser = new BusinessUser(){
            //             IdentityId = user.Id
            //         };

            //         _context.Add(businessUser);
            //         if( await _context.SaveChangesAsync() > 0)
            //         {
            //             relatedUserId = businessUser.Id;
            //         }
            //         break;
            //     default:
            //         return user;
            // }

            // _context.Add(appUser);

            // if(relatedUserId > 0)
            // {
            //     user.RelatedUserClassId = relatedUserId;
            //     result = await _userManager.UpdateAsync(user); 
            //     if(result.Succeeded){
            //         return user;
            //     }
            // }            

            throw new Exception("Failed to save register infromation");
        }



        public async Task<bool> AccountExists(string username, string email)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username || x.Email == email);
        }

        public async Task<bool> UserNameExists(string username, string exceptionId = "")
        {
           return await _context.Users.Where(x => x.Id != exceptionId).AnyAsync(x => x.UserName == username);
        }

        public async Task<bool> EmailExists(string email, string exceptionId = "")
        {
            return await _context.Users.Where(x => x.Id != exceptionId).AnyAsync(x => x.Email == email);
        }

        public async Task<bool> RoleExists(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<Account> GetAccount(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<Account> GetAccountByEmail(string email){
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<Account> GetAccountByUserName(string userName){
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<Account> UpdateAccount(Account user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                string errorString = "";
                foreach (var error in result.Errors)
                {
                    errorString += error + "\n";
                }
                throw new Exception(errorString);
            }
            return user;
        }

        public async Task<IList<string>> GetRolesForAccount(Account user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IdentityResult> AddRoles(Account user, string[] roles)
        {
            await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
            return await this._userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> DelteAccount(Account user)
        {
            return await this._userManager.DeleteAsync(user);
        }

        // public async Task<IdentityResult> DeleteMember(Member member){
        //     var Account = await this.GetAccount(member.IdentityId);
        //     return await this._userManager.DeleteAsync(Account); //Member will be deleted (Cascade delete)
        // }
    }
}