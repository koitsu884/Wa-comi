using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly IAuthRepository _repo;
        public AccountController(IAuthRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}" , Name = "GetAccount")]
        [Authorize]
        public async Task<IActionResult> Get(string id){
           var account = await _repo.GetAccount(id);
           if(account == null)
                return NotFound();

            var accountForReturn = _mapper.Map<AccountForReturnDto>(account);

            return Ok(accountForReturn);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(string id, [FromBody]AccountUpdateDto updateUserDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var account = await _repo.GetAccount(id);
            if(account == null)
                return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( currentUserId != id)
                return Unauthorized();

            if(await _repo.UserNameExists(updateUserDto.UserName, id))
                return BadRequest("そのユーザーネームは既に使用されています");

            if(await _repo.EmailExists(updateUserDto.Email, id))
                return BadRequest("そのEメールは既に登録されています");

             _mapper.Map(updateUserDto, account);

            try{
                await _repo.UpdateAccount(account);
                var returnUser = _mapper.Map<AccountForReturnDto>(account);
                return Ok(returnUser);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
            // if(await _repo.UpdateAppUser(appUser)){
            //     var returnUser = _mapper.Map<AppUserForReturnDto>(appUser);
            //     return Ok(returnUser);
            // }
            // else{
            //     return BadRequest("Something went wrong when updating AppUser");
            // }
        }

        // [HttpDelete("{userId}")]
        // [Authorize(Roles = "Administrator")]
        // public async Task<IActionResult> DeleteAppUser(string userId){
        //     var appUserFromRepo = await this._repo.GetAppUser(userId);
        //     if(appUserFromRepo == null){
        //         return NotFound("The user not found");
        //     }
        //     var result = await this._repo.DelteAppUser(appUserFromRepo);
        //     if(result.Succeeded){
        //         return NoContent();
        //     }
        //     return BadRequest("Failed to delete the user");
        // }

        // [HttpPost("role/{userId}/")]
        // [Authorize(Roles = "Administrator")]
        // public async Task<IActionResult> AddRoleToAppUser(string userId, [FromBody]string[] roles){
        //     if(roles == null)
        //     {
        //         return BadRequest("No role parameter in the body");
        //     }
        //     else
        //     {
        //         foreach(var role in roles){
        //         if(! await _repo.RoleExists(role))
        //             return BadRequest("The role '" + role + "' not exists");
        //         }
        //     }

        //     var user = await _repo.GetAppUser(userId);
        //     if(user == null)
        //         return BadRequest("Not found the user");


        //     var result = _repo.AddRoles(user, roles);
        //     if(result.Result.Succeeded)
        //         return Ok("The roles were successfully added to " + user.UserName);

        //     return BadRequest("Failed to add roles"); 
        // }
    }
}