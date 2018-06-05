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
         public FriendRequestController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}


        [HttpGet("{senderId}/{recipientId}", Name = "GetFriendRequest")]
        public async Task<ActionResult> Get(int senderId, int recipientId)
        {
            var requestFromRepo = await _repo.GetFriendRequest(senderId, recipientId);
            return Ok(_mapper.Map<FriendRequestSentForReturnDto>(requestFromRepo));
        }
        

        [HttpGet("{memberId}/received/{senderId}")]
        public async Task<ActionResult> GetReceivedRequest(int memberId, int senderId){
            var requestFromRepo =await _repo.GetFriendRequestFrom(memberId, senderId);
            return Ok(_mapper.Map<FriendRequestReceivedForReturnDto>(requestFromRepo));
        }

        [HttpGet("{memberId}/received")]
        public async Task<ActionResult> GetReceivedRequests(int memberId)
        {
            var requestFromRepo = await _repo.GetFriendRequestsReceived(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendRequestReceivedForReturnDto>>(requestFromRepo));
        }

        [HttpGet("{memberId}/sent")]
        public async Task<ActionResult> GetSentRequests(int memberId)
        {
            var requestFromRepo = await _repo.GetFriendRequestsSent(memberId);
            return Ok(_mapper.Map<IEnumerable<FriendRequestSentForReturnDto>>(requestFromRepo));
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]FriendRequest model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!await this._repo.MemberExist(model.SenderId))
                return NotFound();

            if(!await this._repo.MemberExist(model.RecipientId))
                return NotFound();

            var requestFromRepo = await this._repo.GetFriendRequest(model.SenderId, model.RecipientId);
            if(requestFromRepo != null)
                return BadRequest("友達リクエスト送信済みです");
            
            if(!await this.MatchMemberWithToken(model.SenderId))
                return Unauthorized();

            _repo.Add(model);
            if(await _repo.SaveAll())
            {
                return CreatedAtRoute("GetFriendRequest", new {senderId = model.SenderId, recipientId = model.RecipientId}, _mapper.Map<FriendRequestSentForReturnDto>(model));
            }
            return BadRequest("友達リクエストの送信に失敗しました");   
        }

        [HttpDelete("{senderId}/{recipientId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int senderId, int recipientId){

            var requestFromRepo = await _repo.GetFriendRequest(senderId, recipientId);
            if(requestFromRepo == null)
                return NotFound();

            if(!await this.MatchMemberWithToken(requestFromRepo.SenderId) && !await this.MatchMemberWithToken(requestFromRepo.RecipientId))
                return Unauthorized();

            _repo.Delete(requestFromRepo);

           if (await _repo.SaveAll())
                return Ok();

            return BadRequest("友達リクエストの削除に失敗しました");
        }
    }
}