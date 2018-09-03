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
    public class FriendRequestController : DataController
    {
        private readonly IDataRepository _dataRepo;
        public FriendRequestController(IAppUserRepository appUserRepo, IDataRepository dataRepo, IMapper mapper) : base(appUserRepo, mapper)
        {
            this._dataRepo = dataRepo;

        }


        [HttpGet("{senderId}/{recipientId}", Name = "GetFriendRequest")]
        public async Task<ActionResult> Get(int senderId, int recipientId)
        {
            var requestFromRepo = await _dataRepo.GetFriendRequest(senderId, recipientId);
            return Ok(_mapper.Map<FriendRequestSentForReturnDto>(requestFromRepo));
        }


        [HttpGet("{memberId}/received/{senderId}")]
        public async Task<ActionResult> GetReceivedRequest(int memberId, int senderId)
        {
            var requestFromRepo = await _dataRepo.GetFriendRequestFrom(memberId, senderId);
            return Ok(_mapper.Map<FriendRequestReceivedForReturnDto>(requestFromRepo));
        }

        [HttpGet("{memberId}/received")]
        public async Task<ActionResult> GetReceivedRequests(int memberId)
        {
            var requestFromRepo = await _dataRepo.GetFriendRequestsReceived(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendRequestReceivedForReturnDto>>(requestFromRepo));
        }

        [HttpGet("{memberId}/sent")]
        public async Task<ActionResult> GetSentRequests(int memberId)
        {
            var requestFromRepo = await _dataRepo.GetFriendRequestsSent(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendRequestSentForReturnDto>>(requestFromRepo));
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]FriendRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this._dataRepo.RecordExist("MemberProfile", model.SenderId))
                return NotFound();

            if (!await this._dataRepo.RecordExist("MemberProfile", model.RecipientId))
                return NotFound();

            var requestFromRepo = await this._dataRepo.GetFriendRequest(model.SenderId, model.RecipientId);
            if (requestFromRepo != null)
                return BadRequest("友達リクエスト送信済みです");

            if (!await this.MatchAppUserWithToken(model.Sender.AppUserId))
                return Unauthorized();

            _dataRepo.Add(model);
            if (await _dataRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetFriendRequest", new { senderId = model.SenderId, recipientId = model.RecipientId }, _mapper.Map<FriendRequestSentForReturnDto>(model));
            }
            return BadRequest("友達リクエストの送信に失敗しました");
        }

        [HttpDelete("{senderId}/{recipientId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int senderId, int recipientId)
        {

            var requestFromRepo = await _dataRepo.GetFriendRequest(senderId, recipientId);
            if (requestFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken(requestFromRepo.Sender.AppUserId) && !await this.MatchAppUserWithToken(requestFromRepo.Recipient.AppUserId))
                return Unauthorized();

            _dataRepo.Delete(requestFromRepo);

            if (await _dataRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("友達リクエストの削除に失敗しました");
        }
    }
}