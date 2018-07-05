using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class MemberProfileController : DataController
    {

        public MemberProfileController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        // [HttpGet]
        // [Authorize]
        // public async Task<IActionResult> GetMembers(UserParams userParams)
        // {
        //     var members = await _repo.GetMembers(userParams);
        //     var membersToReturn = this._mapper.Map<IEnumerable<MemberForListDto>>(members);
        //     return Ok(membersToReturn);
        // }

        [HttpGet("{id}" , Name = "GetMember")]
        public async Task<IActionResult> Get(int id){
           var member = await _repo.GetMemberProfile(id);
           if(member == null)
                return NotFound("メンバーが見つかりませんでした。ID:" + id);

            var memberToReturn = this._mapper.Map<MemberProfileForReturnDto>(member);

            return Ok(memberToReturn);
        }
        

        // [HttpPost]
        // public async Task<IActionResult> Register([FromBody]MemberRegistrationDto model)
        // {
        //     if (await _repo.AppUserExists(model.UserName, model.Email))
        //         ModelState.AddModelError("UserName", "The username or the email already exist");

        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     var appUser = _mapper.Map<AppUser>(model);
            
        //     //Maybe there should be switch case for type of user ex.Customer, Vendor, Admin etc
        //     var member = _mapper.Map<Member>(model);
        //     if(string.IsNullOrEmpty(member.DisplayName)){
        //         member.DisplayName = appUser.UserName;
        //     }
        //     member.Identity = appUser;

        //     member = await _repo.Register(member, model.Password);
        //     await _repo.AddRoles(member.Identity, new string[] {"Member"});
            
        //     var memberForReturn = _mapper.Map<MemberForReturnDto>(member);
        //     return CreatedAtRoute("GetMember", new {id = member.Id}, memberForReturn);
        // }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody]MemberProfileUpdateDto model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memberFromRepo = await _repo.GetMemberProfile(id);
            if(memberFromRepo == null)
                return NotFound($"Could not find member with an ID of {id}");

            // var appUserFromRepo = await _repo.GetAppUser(memberFromRepo.IdentityId);

            // if(appUserFromRepo == null)
            //     return NotFound($"Could not find user with an ID of {id}");

            // if(currentUserId != memberFromRepo.IdentityId)
            if(!await MatchAppUserWithToken(memberFromRepo.AppUserId))
                return Unauthorized();

            _mapper.Map(model, memberFromRepo);
            if(await _repo.SaveAll() > 0)
            {
                return Ok();
            }
            return BadRequest("メンバープロファイル情報の更新に失敗しました");
        }

        // [HttpDelete("{id}")]
        // [Authorize(Roles = "Administrator")]
        // public async Task<IActionResult> DeleteMember(int id){
        //     var memberFromRepo = await this._repo.GetMember(id);
        //     if(memberFromRepo == null){
        //         return NotFound("The user not found");
        //     }
        //     var result = await this._repo.DeleteMember(memberFromRepo);
        //     if(result.Succeeded){
        //         return NoContent();
        //     }
        //     return BadRequest("Failed to delete the user");
        // }
    }
}