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
    public class CircleTopicController : DataWithPhotoController
    {
        private readonly ICircleRepository _repo;
        private readonly ImageFileStorageManager _imageFileStorageManager;

        public CircleTopicController(IAppUserRepository appUserRepository,
                IMapper mapper,
                IPhotoRepository photoRepo,
                ImageFileStorageManager imageFileStorageManager,
                ICircleRepository repo
                ) : base(appUserRepository, mapper, photoRepo)
        {
            _imageFileStorageManager = imageFileStorageManager;
            _repo = repo;
        }

        protected override string GetTableName()
        {
            return "CircleTopics";
        }

        protected override bool MultiPhoto() { return false; }

        [HttpGet("{id}", Name = "GetCircleTopic")]
        public async Task<ActionResult> Get(int id)
        {
            var circleTopicForReturn = _mapper.Map<CircleTopicForReturnDto>(await _repo.GetCircleTopic(id));
            circleTopicForReturn.TopicCommentCounts = await _repo.GetCircleTopicCommentCount(id);
            return Ok(circleTopicForReturn);
        }

        [HttpGet("{circleId}/latest")]
        public async Task<ActionResult> GetLatestCircles(int circleId)
        {
            return Ok(this._mapper.Map<IEnumerable<CircleTopicForReturnDto>>(await _repo.GetLatestCircleTopicList(circleId)));
        }

        [HttpGet("{circleId}/user/{userId}")]
        public async Task<ActionResult> GetByUser(PaginationParams paginationParams, int circleId, int userId)
        {
            var topics = await _repo.GetCircleTopicByUser(paginationParams, circleId, userId);
            Response.AddPagination(topics.CurrentPage, topics.PageSize, topics.TotalCount, topics.TotalPages);
            return Ok(this._mapper.Map<IEnumerable<CircleTopicForReturnDto>>(topics));
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult> GetTopicComments(PaginationParams paginationParams, int id)
        {
            var topics = await _repo.GetCircleTopicCommentList(paginationParams, id);
            Response.AddPagination(topics.CurrentPage, topics.PageSize, topics.TotalCount, topics.TotalPages);
            var topicsCommentsForReturn = this._mapper.Map<IEnumerable<CircleTopicCommentForReturnDto>>(topics);
            foreach(var topicComment in topicsCommentsForReturn){
                topicComment.ReplyCount = await _repo.GetCircleTopicCommentReplyCount(topicComment.Id);
            }
            return Ok(topicsCommentsForReturn);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleTopicUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId) || !await _repo.IsMember(model.AppUserId, model.CircleId))
                return Unauthorized();

            var newTopic = this._mapper.Map<CircleTopic>(model);
            _repo.Add(newTopic);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircleTopic", new { id = newTopic.Id }, _mapper.Map<CircleTopicForReturnDto>(await _repo.GetCircleTopic(newTopic.Id)));
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]CircleTopicUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var circleTopicFromRepo = await this._repo.GetCircleTopic(model.Id);
            if (circleTopicFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleTopicFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, circleTopicFromRepo);

            try
            {
                await _repo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update attraction: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleTopicFromRepo = await _repo.GetCircleTopic(id);
            if (circleTopicFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleTopicFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            var allTopicPhotos = await _repo.GetAllCommentPhotosForCircleTopic(circleTopicFromRepo.Id);
            _repo.Delete(circleTopicFromRepo);
            await _repo.SaveAll();

            if(circleTopicFromRepo.PhotoId != null)
            {
                _repo.Delete(circleTopicFromRepo.Photo);
                var task = this._imageFileStorageManager.DeleteImageFileAsync(circleTopicFromRepo.Photo);
            }

            foreach (var topicPhoto in allTopicPhotos)
            {
                _repo.Delete(topicPhoto);
                var task = this._imageFileStorageManager.DeleteImageFileAsync(topicPhoto);
            }

            await _repo.SaveAll();
            return Ok();
        }

        [HttpGet("{id}/photo", Name = "GetCircleTopicPhotos")]
        public async Task<ActionResult> GetCircleTopicPhotos(int id)
        {
            return await base.GetPhotos(id);
        }

        [HttpPost("{id}/photo")]
        [Authorize]
        public async Task<ActionResult> AddCircleTopicPhotos(int id, List<IFormFile> files)
        {
            return await AddPhotos(id, files, "GetCircleTopicPhotos");
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteCircleTopicPhoto(int id, int photoId)
        {
            return await DeletePhoto(id, photoId);
        }
    }
}