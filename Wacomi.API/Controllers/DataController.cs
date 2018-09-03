using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;

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


        protected async Task<bool> MatchAppUserWithToken(int appUserId){
            var appUser = await _appUserRepo.GetAppUser(appUserId);
            if(appUser != null && appUser.AccountId == User.FindFirst(ClaimTypes.NameIdentifier).Value){
                return true;
            }
            return false;
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