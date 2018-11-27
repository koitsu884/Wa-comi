using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;
using Wacomi.API.Models.Circles;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleEventCommentReplyController : DataController
    {
        private readonly ICircleRepository _repo;
        private readonly INotificationRepository _notificationRepo;
        public CircleEventCommentReplyController(IAppUserRepository appUserRepository, IMapper mapper, ICircleRepository repo, INotificationRepository notificationRepo) : base(appUserRepository, mapper)
        {
            this._repo = repo;
            this._notificationRepo = notificationRepo;
        }

         [HttpGet("{id}", Name = "GetCircleEventCommentReply")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(_mapper.Map<CommentForReturnDto>(await _repo.GetCircleEventCommentReply(id)));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleEventCommentReply model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var circleEventComment = await _repo.GetCircleEventComment(model.CommentId);
            if(circleEventComment == null)
                return NotFound();
            if (!await this.MatchAppUserWithToken(model.AppUserId)  || !await _repo.IsMember((int)model.AppUserId, circleEventComment.CircleId))
                return Unauthorized();

            model.CircleId = circleEventComment.CircleId;
            // var newTopic = this._mapper.Map<CircleEvent>(model);
            _repo.Add(model);
            await _repo.SaveAll();
            if(circleEventComment.AppUserId == model.AppUserId) //Comment owner
                await _notificationRepo.AddNotification(NotificationEnum.NewCircleEventReplyByOwner, (int)model.AppUserId, model);
            else
                await _notificationRepo.AddNotification(NotificationEnum.NewCircleEventReplyByMember, (int)model.AppUserId, model);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircleEventCommentReply", new { id = model.Id }, _mapper.Map<CommentForReturnDto>(model));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleEventCommentReplyFromRepo = await _repo.GetCircleEventCommentReply(id);
            if (circleEventCommentReplyFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleEventCommentReplyFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(circleEventCommentReplyFromRepo);
            await _repo.SaveAll();
            return Ok();
        }
    }
}