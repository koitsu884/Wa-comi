using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IDataProtector _protector;
        private readonly INotificationRepository _notificationRepo;
        public MessageController(
            IDataRepository repo, 
            IMapper mapper, 
            IConfiguration config, 
            IDataProtectionProvider provider,
            INotificationRepository notificationRepo
            ) : base(repo, mapper) {
            this._protector = provider.CreateProtector(config.GetSection("AppSettings:Token").Value);
            this._notificationRepo = notificationRepo;
        }
        
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _repo.GetMessage(id);
            message.Content = this._protector.Unprotect(message.Content);
            return Ok(message);
        }

        [HttpGet("{userId}/received")]
        public async Task<ActionResult> GetReceivedMessages(PaginationParams paginationParams, int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages =  await _repo.GetReceivedMessages(paginationParams, userId);
            foreach(var message in messages){
                message.Content = this._protector.Unprotect(message.Content);
            }
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

         [HttpGet("{userId}/sent")]
        public async Task<ActionResult> GetSentMessages(PaginationParams paginationParams, int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetSentMessages(paginationParams, userId);
            foreach(var message in messages){
                message.Content = this._protector.Unprotect(message.Content);
            }
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/received/{senderId}")]
        public async Task<ActionResult> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetReceivedMessagesFrom(paginationParams, userId, senderId);
            foreach(var message in messages){
                message.Content = this._protector.Unprotect(message.Content);
            }
            Response.AddPagination(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return Ok(_mapper.Map<IEnumerable<MessageForReturnDto>>(messages));
        }

        [HttpGet("{userId}/sent/{recipientId}")]
        public async Task<ActionResult> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var messages = await _repo.GetMessagesSentTo(paginationParams, userId, recipientId);
            foreach(var message in messages){
                message.Content = this._protector.Unprotect(message.Content);
            }
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
            model.Content = this._protector.Protect(model.Content);
            _repo.Add(model);
            if(await _repo.SaveAll() > 0){
                await _notificationRepo.AddNotificationNewMessage(model);
                await _repo.SaveAll();
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