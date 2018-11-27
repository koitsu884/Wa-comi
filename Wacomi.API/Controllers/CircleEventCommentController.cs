using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Dto.Circle;
using Wacomi.API.Models.Circles;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleEventCommentController: DataController
    {
        private readonly ICircleRepository _repo;

        public CircleEventCommentController(IAppUserRepository appUserRepository, 
                IMapper mapper,
                ICircleRepository repo) : base(appUserRepository, mapper)
        {
            this._repo = repo;
        }


         [HttpGet("{id}", Name = "GetCircleEventComment")]
        public async Task<ActionResult> Get(int id)
        {
            var circleEventCommentForReturn = _mapper.Map<CircleEventCommentForReturnDto>(await _repo.GetCircleEventComment(id));
            circleEventCommentForReturn.ReplyCount = await _repo.GetCircleEventCommentReplyCount(id);
            return Ok(circleEventCommentForReturn);
        }

        [HttpGet("{id}/replies", Name = "GetCircleEventCommentReplies")]
        public async Task<ActionResult> GetReplies(int id)
        {
            var topicRepliesFromRepo = await _repo.GetCircleEventCommentReplies(id);
 
            return Ok(_mapper.Map<IEnumerable<CommentForReturnDto>>(topicRepliesFromRepo));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]UserCommentUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var circleEvent = await _repo.GetCircleEvent(model.OwnerRecordId);
            if(circleEvent == null)
                return NotFound();
            if (!await this.MatchAppUserWithToken(model.AppUserId) 
            || (!circleEvent.IsPublic && !await _repo.IsMember((int)model.AppUserId, circleEvent.CircleId)))
                return Unauthorized();

             var newComment = this._mapper.Map<CircleEventComment>(model);
             newComment.CircleId = circleEvent.CircleId;
            _repo.Add(newComment);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircleEventComment", new { id = model.Id }, _mapper.Map<CircleEventCommentForReturnDto>(newComment));
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]UserCommentUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var circleEventCommentFromRepo = await this._repo.GetCircleEventComment(model.Id);
            if (circleEventCommentFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleEventCommentFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            circleEventCommentFromRepo.Comment = model.Comment;
            //_mapper.Map(model, circleEventCommentFromRepo);

            try
            {
                await _repo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update circle topic comment: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleEventCommentFromRepo = await _repo.GetCircleEventComment(id);
            if (circleEventCommentFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleEventCommentFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(circleEventCommentFromRepo);
            await _repo.SaveAll();
            return Ok();
        }
    }
}