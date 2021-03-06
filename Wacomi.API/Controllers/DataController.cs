using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    public class DataController : Controller
    {
        protected readonly IAppUserRepository _appUserRepo;
        protected readonly IMapper _mapper;
        public DataController(IAppUserRepository appUserRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._appUserRepo = appUserRepository;
        }


        protected async Task<bool> MatchAppUserWithToken(int? appUserId){
            if(appUserId == null)
                return false;
            var appUser = await _appUserRepo.GetAppUser((int)appUserId);
            if(appUser != null && appUser.AccountId == User.FindFirst(ClaimTypes.NameIdentifier).Value){
                return true;
            }
            return false;
        }

        protected async Task<AppUser> GetLoggedInUserAsync(){
            var loggedInUser =  User.FindFirst(ClaimTypes.NameIdentifier);
            string loginUserAccountId = null;
            if(loggedInUser != null)
            {
                loginUserAccountId = loggedInUser.Value;
                if(loginUserAccountId == null)
                    return null;
            }
            return await _appUserRepo.GetAppUserByAccountId(loginUserAccountId);
        }

        protected async Task<MemberProfile> GetLoggedInMemberProfileAsync(){
            var loggedInUser =  User.FindFirst(ClaimTypes.NameIdentifier);
            string loginUserAccountId = null;
            if(loggedInUser != null)
            {
                loginUserAccountId = loggedInUser.Value;
                if(loginUserAccountId == null)
                    return null;
            }
            return await _appUserRepo.GetMemberProfileByAccountId(loginUserAccountId);
        }

         protected async Task<bool> MatchMemberWithToken(int memberProfileId){
            var member = await _appUserRepo.GetMemberProfile(memberProfileId);
            if(member != null && member.AppUser.AccountId == User.FindFirst(ClaimTypes.NameIdentifier).Value){
                return true;
            }
            return false;
        }
        protected bool MatchUserWithToken(string userId){
            if(userId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;
            return false;
        }
    }
}