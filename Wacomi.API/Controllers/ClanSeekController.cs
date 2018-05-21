using System.Collections.Generic;
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
    public class ClanSeekController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public ClanSeekController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetClanSeek")]
        public async Task<ActionResult> GetClanSeek(int id)
        {
            return Ok(await _repo.GetClanSeek(id));
        }

        [HttpGet()]
        public async Task<ActionResult> GetClanSeeks(int? categoryId, int? cityId)
        {
            var clanSeeks = await _repo.GetClanSeeks(categoryId, cityId);
            var clanSeeksForReturn = this._mapper.Map<IEnumerable<ClanSeekForReturnDto>>(clanSeeks);
            return Ok(clanSeeksForReturn);
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetClanSeekCategories(){
            return Ok(await _repo.GetClanSeekCategories());
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddClanSeek([FromBody]ClanSeekForCreationDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newClanSeek = this._mapper.Map<ClanSeek>(model);
            _repo.Add(newClanSeek);
            if(await _repo.SaveAll()){
                return CreatedAtRoute("GetClanSeek", new {id = newClanSeek.Id}, newClanSeek); 
            }
            return BadRequest("Failed to add clanseek");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateClanSeek(int id, [FromBody]ClanSeekUpdateDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clanSeekFromRepo = await this._repo.GetClanSeek(id);
            if(clanSeekFromRepo == null){
                return NotFound();
            }
            if(!await MatchUserWithToken(clanSeekFromRepo.MemberId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, clanSeekFromRepo);
            if(await _repo.SaveAll())
            {
                return Ok();
            }
            return BadRequest("Failed to update the clan seek");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClanSeek(int id){
            var clanSeek = await _repo.GetClanSeek(id);
            if(clanSeek == null){
                return NotFound();
            }
            if(!await MatchUserWithToken(clanSeek.MemberId))
            {
                return Unauthorized();
            }
            _repo.Delete(clanSeek);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the clan seek");
        }

        private async Task<bool> MatchUserWithToken(int memberId)
        {
            var member = await _repo.GetMember(memberId);
            if(member.IdentityId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;
            return false;
        }
    }
}