using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
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
        public async Task<ActionResult> GetReceivedMessages(PaginationParams paginationParams, int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages =  await _repo.GetReceivedMessages(paginationParams, userId);
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

         [HttpGet("{userId}/sent")]
        public async Task<ActionResult> GetSentMessages(PaginationParams paginationParams, int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetSentMessages(paginationParams, userId);
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        // [HttpGet("{userId}/received/list")]
        // public async Task<ActionResult> GetLatestReceivedMessages(int userId)
        // {
        //     if(!await this.MatchAppUserWithToken(userId))
        //         return Unauthorized();
        //     var messages =  _repo.GetLatestReceivedMessages(userId);
        //     return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        // }

        //  [HttpGet("{userId}/sent/list")]
        // public async Task<ActionResult> GetLatestSentMessages(int userId)
        // {
        //     if(!await this.MatchAppUserWithToken(userId))
        //         return Unauthorized();
        //     var messages = await _repo.GetLatestSentMessages(userId);
        //     return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        // }

        [HttpGet("{userId}/received/{senderId}")]
        public async Task<ActionResult> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetReceivedMessagesFrom(paginationParams, userId, senderId);
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/sent/{recipientId}")]
        public async Task<ActionResult> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetMessagesSentTo(paginationParams, userId, recipientId);
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/new")]
        public async Task<ActionResult> GetNewMessagesCount(int userId)
        {
            return Ok(await _repo.GetNewMessagesCount(userId));
        } 

        [HttpPost()]
        public async Task<ActionResult> SendMessageTo([FromBody]Message model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await _repo.RecordExist("AppUser", model.SenderId)){
                return NotFound("アカウントが見つかりません ID:" + model.SenderId);
            }

            if(!await _repo.RecordExist("AppUser", model.RecipientId)){
                return NotFound("送信相手のアカウントが見つかりません ID:" + model.RecipientId);
            }

            if(!await MatchAppUserWithToken(model.SenderId))
            {
                return Unauthorized();
            }
            _repo.Add(model);
            if(await _repo.SaveAll() > 0){
                return CreatedAtRoute("GetMessage", new {id = model.Id}, new {}); 
            }
            return BadRequest("Failed to add message");
        }

        [HttpPut("{id}/read")]
        public async Task<ActionResult> SetReadFlag(int id){
            var message = await _repo.GetMessage(id);
            if(message == null){
                return NotFound();
            }
            if(!await MatchAppUserWithToken(message.RecipientId))
            {
                return Unauthorized();
            }
            message.IsRead = true;

            await _repo.SaveAll();
            return Ok(await _repo.GetNewMessagesCount(message.RecipientId));
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

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the message");
        }
    }
}