using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AppUserController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IMapper _mapper;
        public AppUserController(IAuthRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAppUser(string id, [FromBody]AppUserUpdateDto updateUserDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var appUser = await _repo.GetAppUser(id);
            if(appUser == null)
                return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if( currentUserId != id)
                return Unauthorized();

            if(await _repo.UserNameExists(updateUserDto.UserName, id))
                return BadRequest("そのユーザーネームは既に使用されています");

            if(await _repo.EmailExists(updateUserDto.Email, id))
                return BadRequest("そのEメールは既に登録されています");

             _mapper.Map(updateUserDto, appUser);

            try{
                await _repo.UpdateAppUser(appUser);
                var returnUser = _mapper.Map<AppUserForReturnDto>(appUser);
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
    }
}