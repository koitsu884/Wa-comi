using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleTopicCommentController : DataWithPhotoController
    {
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ICircleRepository _repo;

        public CircleTopicCommentController(IAppUserRepository appUserRepository, 
                IMapper mapper, 
                IPhotoRepository photoRepo,
                ImageFileStorageManager imageFileStorageManager,
                ICircleRepository repo) : base(appUserRepository, mapper, photoRepo)
        {
            this._imageFileStorageManager = imageFileStorageManager;
            this._repo = repo;
        }

        protected override string GetTableName()
        {
            return "CircleTopicComments";
        }

        protected override bool MultiPhoto(){return false;}

         [HttpGet("{id}", Name = "GetCircleTopicComment")]
        public async Task<ActionResult> Get(int id)
        {
            var circleTopicCommentForReturn = _mapper.Map<CircleTopicCommentForReturnDto>(await _repo.GetCircleTopicComment(id));
            circleTopicCommentForReturn.ReplyCount = await _repo.GetCircleTopicCommentReplyCount(id);
            return Ok(circleTopicCommentForReturn);
        }

        [HttpGet("{id}/replies", Name = "GetCircleTopicCommentReplies")]
        public async Task<ActionResult> GetReplies(int id)
        {
            var topicRepliesFromRepo = await _repo.GetCircleTopicCommentReplies(id);
 
            return Ok(_mapper.Map<IEnumerable<CommentForReturnDto>>(topicRepliesFromRepo));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleTopicComment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var circleTopic = await _repo.GetCircleTopic(model.CircleTopicId);
            if(circleTopic == null)
                return NotFound();
            if (!await this.MatchAppUserWithToken(model.AppUserId) || !await _repo.IsMember((int)model.AppUserId, circleTopic.CircleId))
                return Unauthorized();

            model.CircleId = circleTopic.CircleId;
            // var newTopic = this._mapper.Map<CircleTopic>(model);
            _repo.Add(model);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircleTopicComment", new { id = model.Id }, _mapper.Map<CircleTopicCommentForReturnDto>(model));
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]CircleTopicCommentUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var circleTopicCommentFromRepo = await this._repo.GetCircleTopicComment(model.Id);
            if (circleTopicCommentFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleTopicCommentFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, circleTopicCommentFromRepo);

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
            var circleTopicCommentFromRepo = await _repo.GetCircleTopicComment(id);
            if (circleTopicCommentFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleTopicCommentFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(circleTopicCommentFromRepo);
            await _repo.SaveAll();

            if(circleTopicCommentFromRepo.PhotoId != null)
            {
                _repo.Delete(circleTopicCommentFromRepo.Photo);
                var task = this._imageFileStorageManager.DeleteImageFileAsync(circleTopicCommentFromRepo.Photo);
            }

            await _repo.SaveAll();
            return Ok();
        }

        [HttpGet("{id}/photo", Name = "GetCircleTopicCommentPhotos")]
        public async Task<ActionResult> GetCircleTopicCommentPhotos(int id)
        {
            return await base.GetPhotos(id);
        }

        [HttpPost("{id}/photo")]
        [Authorize]
        public async Task<ActionResult> AddCircleTopicCommentPhotos(int id, List<IFormFile> files)
        {
            return await AddPhotos(id, files, "GetCircleTopicCommentPhotos");
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteCircleTopicCommentPhoto(int id, int photoId)
        {
            return await DeletePhoto(id, photoId);
        }
    }
}