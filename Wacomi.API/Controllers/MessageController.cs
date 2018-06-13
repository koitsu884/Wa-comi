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
    [Authorize]
    public class MessageController : DataController
    {
        public MessageController(IDataRepository repo, IMapper mapper) : base(repo, mapper) {}
        
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _repo.GetMessage(id));
        }

        [HttpGet("{userId}/received")]
        public async Task<ActionResult> GetReceivedMessages(int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages =  await _repo.GetReceivedMessages(userId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

         [HttpGet("{userId}/sent")]
        public async Task<ActionResult> GetSentMessages(int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetSentMessages(userId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/received/list")]
        public async Task<ActionResult> GetLatestReceivedMessages(int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages =  _repo.GetLatestReceivedMessages(userId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

         [HttpGet("{userId}/sent/list")]
        public async Task<ActionResult> GetLatestSentMessages(int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetLatestSentMessages(userId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/received/{senderId}")]
        public async Task<ActionResult> GetReceivedMessagesFrom(int userId, int senderId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetReceivedMessagesFrom(userId, senderId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/sent/{recipientId}")]
        public async Task<ActionResult> GetMessagesSentTo(int userId, int recipientId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetMessagesSentTo(userId, recipientId);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpPost()]
        public async Task<ActionResult> SendMessageTo([FromBody]Message model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await _repo.AppUserExist(model.SenderId)){
                return NotFound("アカウントが見つかりません ID:" + model.SenderId);
            }

            if(!await _repo.AppUserExist(model.RecipientId)){
                return NotFound("送信相手のアカウントが見つかりません ID:" + model.RecipientId);
            }

            if(!await MatchAppUserWithToken(model.SenderId))
            {
                return Unauthorized();
            }
            _repo.Add(model);
            if(await _repo.SaveAll()){
                return CreatedAtRoute("GetMessage", new {id = model.Id}, new {}); 
            }
            return BadRequest("Failed to add message");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id){
            var message = await _repo.GetMessage(id);
            if(message == null){
                return NotFound();
            }
            if(!await MatchAppUserWithToken(message.SenderId))
            {
                return Unauthorized();
            }
            _repo.Delete(message);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the message");
        }
    }
}