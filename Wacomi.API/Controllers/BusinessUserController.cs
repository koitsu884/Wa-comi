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
    public class BusinessUserController : AccountsController
    {
        public BusinessUserController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{id}" , Name = "GetBusinessUser")]
        [Authorize]
        public async Task<IActionResult> GetBusinessUser(int id){
           var bisuser = await _repo.GetBusinessUser(id);
           if(bisuser == null)
                return NotFound();

            var bisUserToReturn = _mapper.Map<BusinessUserForReturnDto>(bisuser);

            return new ObjectResult(bisUserToReturn);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusinessUser(int id, [FromBody]BusinessUserUpdateDto model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bisUserFromRepo = await _repo.GetBusinessUser(id);
            if(bisUserFromRepo == null)
                return NotFound($"Could not find user with an ID of {id}");

            if(currentUserId != bisUserFromRepo.IdentityId)
                return Unauthorized();

            _mapper.Map(model, bisUserFromRepo);
            if(await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to update the user");
        }
    }
}