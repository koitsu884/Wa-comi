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
    public class CircleTopicCommentReplyController : DataController
    {
        private readonly ICircleRepository _repo;
        public CircleTopicCommentReplyController(IAppUserRepository appUserRepository, IMapper mapper, ICircleRepository repo) : base(appUserRepository, mapper)
        {
            this._repo = repo;
        }

         [HttpGet("{id}", Name = "GetCircleTopicCommentReply")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(_mapper.Map<CommentForReturnDto>(await _repo.GetCircleTopicCommentReply(id)));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleTopicCommentReply model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var circleTopicComment = await _repo.GetCircleTopicComment(model.CommentId);
            if(circleTopicComment == null)
                return NotFound();
            if (!await this.MatchAppUserWithToken(model.AppUserId)  || !await _repo.IsMember((int)model.AppUserId, circleTopicComment.CircleId))
                return Unauthorized();

            model.CircleId = circleTopicComment.CircleId;
            // var newTopic = this._mapper.Map<CircleTopic>(model);
            _repo.Add(model);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircleTopicCommentReply", new { id = model.Id }, _mapper.Map<CommentForReturnDto>(model));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleTopicCommentReplyFromRepo = await _repo.GetCircleTopicCommentReply(id);
            if (circleTopicCommentReplyFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleTopicCommentReplyFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(circleTopicCommentReplyFromRepo);
            await _repo.SaveAll();
            return Ok();
        }
    }
}