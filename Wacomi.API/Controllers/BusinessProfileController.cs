using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class BusinessProfileController : DataController
    {
        public BusinessProfileController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{id}" , Name = "GetBusinessProfile")]
        public async Task<IActionResult> Get(int id){
           var bisuser = await _repo.GetBusinessProfile(id);
           if(bisuser == null)
                return NotFound();

            var bisUserToReturn = _mapper.Map<BusinessProfileForReturnDto>(bisuser);

            return new ObjectResult(bisUserToReturn);
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBusinessUser(int id, [FromBody]BusinessProfileUpdateDto model){
             if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var businessFromRepo = await _repo.GetBusinessProfile(id);
            if(businessFromRepo == null)
                return NotFound($"Could not find business with an ID of {id}");

            // var appUserFromRepo = await _repo.GetAppUser(memberFromRepo.IdentityId);

            // if(appUserFromRepo == null)
            //     return NotFound($"Could not find user with an ID of {id}");

            // if(currentUserId != memberFromRepo.IdentityId)
            if(!await MatchAppUserWithToken(businessFromRepo.AppUserId))
                return Unauthorized();

            _mapper.Map(model, businessFromRepo);
            if(await _repo.SaveAll() > 0)
            {
                return Ok();
            }
            return BadRequest("ビジネスプロファイル情報の更新に失敗しました");
        }
    }
}