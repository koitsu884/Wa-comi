using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;

namespace Wacomi.API.Controllers
{
    public class DataController : Controller
    {
        protected readonly IDataRepository _repo;
        protected readonly IMapper _mapper;
        public DataController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        protected async Task<bool> MatchMemberWithToken(int memberId){
            var member = await _repo.GetMember(memberId);
            if(member.IdentityId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;

            return false;
        }
    }
}