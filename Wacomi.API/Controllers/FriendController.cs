using System.Collections.Generic;
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
    public class FriendController : DataController
    {
        public FriendController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{memberId}/{friendId}", Name = "GetFriend")]
        public async Task<ActionResult> Get(int meberId, int friendId)
        {
            var friendFromRepo = await _repo.GetFriend(meberId, friendId);
            return Ok(_mapper.Map<FriendForReturnDto>(friendFromRepo));
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult> Get(int memberId)
        {
            var friendsFromRepo = await _repo.GetFriends(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendForReturnDto>>(friendsFromRepo));
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]Friend model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await this._repo.MemberProfileExist(model.MemberId))
                return NotFound();

            if(!await this._repo.MemberProfileExist(model.FriendMemberid))
                return NotFound();

            var friendFromRepo = await _repo.GetFriend(model.MemberId, model.FriendMemberid);
            if(friendFromRepo != null){
                return BadRequest("既に友達になっています");
            }
            
            if(!await this.MatchAppUserWithToken(model.Member.AppUserId) && !await this.MatchAppUserWithToken(model.FriendMember.AppUserId))
                return Unauthorized();

            _repo.Add(model);

            if(await _repo.SaveAll())
            {
                return CreatedAtRoute("GetFriend", new {memberId = model.MemberId, friendId = model.FriendMemberid}, _mapper.Map<FriendForReturnDto>(model));
            }
            return BadRequest("友達の追加に失敗しました");   
        }

        [HttpDelete("{memberId}/{friendId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int memberId, int friendId){

            var friendFromRepo = await _repo.GetFriend(memberId, friendId);
            if(friendFromRepo == null)
                return NotFound();

            if(!await this.MatchMemberWithToken(memberId) && !await this.MatchMemberWithToken(friendId))
                return Unauthorized();

            _repo.Delete(friendFromRepo);

           if (await _repo.SaveAll())
                return Ok();

            return BadRequest("友達の削除に失敗しました");
        }
    }
}