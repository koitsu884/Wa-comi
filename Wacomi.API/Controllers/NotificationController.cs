using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : DataController
    {
        private readonly INotificationRepository _notificationRepo;
        public NotificationController(IDataRepository repo, IMapper mapper, INotificationRepository notificationRepo) : base(repo, mapper)
        {
            this._notificationRepo = notificationRepo;
        }

        [HttpGet("{userId}", Name = "GetNotifications")]
        public async Task<ActionResult> Get(int userId)
        {
            if (!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            return Ok(await _notificationRepo.GetNotifications(userId));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var notification = await this._notificationRepo.GetNotification(id);
            if (notification == null)
                return NotFound();
            if (!await this.MatchAppUserWithToken(notification.AppUserId))
                return Unauthorized();
            _notificationRepo.Delete(notification);
            await _notificationRepo.SaveAll();
            return Ok();
        }

        [HttpDelete("{appUserId}/all")]
        public async Task<ActionResult> DeleteAll(int appUserId)
        {
            if (!await this.MatchAppUserWithToken(appUserId))
                return Unauthorized();
            _notificationRepo.DeleteAllNotifications(appUserId);
            await _notificationRepo.SaveAll();
            return Ok();
        }

        [HttpPost("test")]
        [AllowAnonymous]
        public async Task<IActionResult> PostTest([FromBody]Notification model)
        {
            //  await _repo.AddNotification(model.AppUserId, model.RecordType, model.RecordId);
            var topicComment = await this._repo.GetTopicComment(model.RecordId);
            if (topicComment == null) return NotFound();
            if (topicComment.AppUserId == model.AppUserId)
                await this._notificationRepo.AddNotificationRepliedForTopicComment(topicComment);
            else
                await this._notificationRepo.AddNotificationNewPostOnTopicComment(model.AppUserId, topicComment);
            await _repo.SaveAll();
            return Ok();
        }

    }
}