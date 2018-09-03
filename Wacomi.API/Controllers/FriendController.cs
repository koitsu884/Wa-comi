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
        private readonly IDataRepository _dataRepo;
        public FriendController(IAppUserRepository appUserRepo, IDataRepository dataRepo, IMapper mapper) : base(appUserRepo, mapper)
        {
            this._dataRepo = dataRepo;
        }

        [HttpGet("{memberId}/{friendId}", Name = "GetFriend")]
        public async Task<ActionResult> Get(int meberId, int friendId)
        {
            var friendFromRepo = await _dataRepo.GetFriend(meberId, friendId);
            return Ok(_mapper.Map<FriendForReturnDto>(friendFromRepo));
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult> Get(int memberId)
        {
            var friendsFromRepo = await _dataRepo.GetFriends(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendForReturnDto>>(friendsFromRepo));
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]Friend model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await this._dataRepo.RecordExist("MemberProfile", model.MemberId))
                return NotFound();

            if (!await this._dataRepo.RecordExist("MemberProfile", model.FriendMemberid))
                return NotFound();

            var friendFromRepo = await _dataRepo.GetFriend(model.MemberId, model.FriendMemberid);
            if (friendFromRepo != null)
            {
                return BadRequest("既に友達になっています");
            }

            if (!await this.MatchAppUserWithToken(model.Member.AppUserId) && !await this.MatchAppUserWithToken(model.FriendMember.AppUserId))
                return Unauthorized();

            _dataRepo.Add(model);

            if (await _dataRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetFriend", new { memberId = model.MemberId, friendId = model.FriendMemberid }, _mapper.Map<FriendForReturnDto>(model));
            }
            return BadRequest("友達の追加に失敗しました");
        }

        [HttpDelete("{memberId}/{friendId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int memberId, int friendId)
        {

            var friendFromRepo = await _dataRepo.GetFriend(memberId, friendId);
            if (friendFromRepo == null)
                return NotFound();

            if (!await this.MatchMemberWithToken(memberId) && !await this.MatchMemberWithToken(friendId))
                return Unauthorized();

            _dataRepo.Delete(friendFromRepo);

            if (await _dataRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("友達の削除に失敗しました");
        }
    }
}