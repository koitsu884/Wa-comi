using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Dto.Circle;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleEventController : DataWithPhotoController
    {
        private readonly ICircleRepository _repo;
        private readonly INotificationRepository _notificationRepo;
        private readonly ILogger<CircleEventController> _logger;
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private static readonly int CIRCLE_EVENT_MAX = 50;

        public CircleEventController(ICircleRepository repo,
            INotificationRepository notificationRepo,
            ILogger<CircleEventController> logger,
            IAppUserRepository appUserRepository,
            IMapper mapper,
            ImageFileStorageManager imageFileStorageManager,
            IPhotoRepository photoRepo) : base(appUserRepository, mapper, photoRepo)
        {
            this._logger = logger;
            this._repo = repo;
            this._notificationRepo = notificationRepo;
            this._imageFileStorageManager = imageFileStorageManager;
        }

        protected override string GetTableName()
        {
            return "CircleEvents";
        }

        [HttpGet("{id}", Name = "GetCircleEvent")]
        public async Task<ActionResult> Get(int id)
        {
            var circleEventForReturn = _mapper.Map<CircleEventForReturnDto>(await _repo.GetCircleEvent(id));
            if (circleEventForReturn == null)
                return NotFound();
            //circleEventForReturn.EventCommentCounts = await _repo.GetCircleEventCommentCount(id);
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser != null)
            {
                var myParticipation = await _repo.GetCircleEventParticipation(loggedInUser.Id, id);
                circleEventForReturn.MyStatus = myParticipation != null ? (CircleEventParticipationStatus?)myParticipation.Status : null;
                circleEventForReturn.IsCircleMember = await _repo.IsMember(loggedInUser.Id, circleEventForReturn.CircleId);
            }

            return Ok(circleEventForReturn);
        }

        [HttpGet()]
        public async Task<ActionResult> GetCircleEvents(PaginationParams paginationParams, DateTime? fromDate = null, int circleId = 0, int circleCategoryId = 0, int cityId = 0, int appUserId = 0)
        {
            var events = await _repo.GetCircleEvents(paginationParams, fromDate != null ? (DateTime)fromDate : default(DateTime), circleId, circleCategoryId, cityId, appUserId);
            Response.AddPagination(events.CurrentPage, events.PageSize, events.TotalCount, events.TotalPages);
            var loggedInUser = await GetLoggedInUserAsync();
            if (loggedInUser != null)
            {
                foreach (var temp in events)
                {
                    var myParticipation = temp.CircleEventParticipations.FirstOrDefault(cep => cep.AppUserId == loggedInUser.Id);
                    if (myParticipation != null)
                    {
                        temp.MyStatus = myParticipation != null ? (CircleEventParticipationStatus?)myParticipation.Status : null;
                    }
                }
            }

            return Ok(_mapper.Map<IEnumerable<CircleEventForReturnDto>>(events));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleEventUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId) || !await _repo.IsOwner((int)model.AppUserId, model.CircleId))
                return Unauthorized();

            var newEvent = this._mapper.Map<CircleEvent>(model);
            _repo.Add(newEvent);
            CircleEvent oldestEvent = null;
            if (await _repo.GetCircleEventCount(model.CircleId) >= CIRCLE_EVENT_MAX)
            {
                oldestEvent = await _repo.GetOldestCircleEvent(model.CircleId);
                _repo.Delete(oldestEvent);
            }
            await _repo.SaveAll();
            if (oldestEvent != null)
            {
                await this.deleteAttachedPhotos(oldestEvent.Photos);
            }

            return CreatedAtRoute("GetCircleEvent", new { id = newEvent.Id }, _mapper.Map<CircleEventForReturnDto>(await _repo.Get<CircleEvent>(newEvent.Id)));
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]CircleEventUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var CircleEventFromRepo = await this._repo.Get<CircleEvent>(model.Id);
            if (CircleEventFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)CircleEventFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, CircleEventFromRepo);

            try
            {
                await _repo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update circle event: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleEventFromRepo = await _repo.Get<CircleEvent>(id);
            if (circleEventFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)circleEventFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(circleEventFromRepo);
            await _repo.SaveAll();
            await this.deleteAttachedPhotos(circleEventFromRepo.Photos);

            return Ok();
        }

         [HttpGet("{id}/comments/{commentId}")]
        public async Task<ActionResult> GetEventComment(int id, int commentId)
        {
            var comment = await _repo.GetCircleEventComment(commentId);
            var eventCommentForReturn = this._mapper.Map<CircleEventCommentForReturnDto>(comment);
            eventCommentForReturn.ReplyCount = await _repo.GetCircleEventCommentReplyCount(comment.Id);
            return Ok(eventCommentForReturn);
        }

        [HttpGet("{id}/comments")]
        public async Task<ActionResult> GetEventComments(PaginationParams paginationParams, int id)
        {
            var eventComments = await _repo.GetCircleEventCommentList(paginationParams, id);
            Response.AddPagination(eventComments.CurrentPage, eventComments.PageSize, eventComments.TotalCount, eventComments.TotalPages);
            var eventCommentsForReturn = this._mapper.Map<IEnumerable<CircleEventCommentForReturnDto>>(eventComments);
            foreach(var topicComment in eventCommentsForReturn){
                topicComment.ReplyCount = await _repo.GetCircleEventCommentReplyCount(topicComment.Id);
            }
            return Ok(eventCommentsForReturn);
        }

        [HttpGet("{id}/photo", Name = "GetCircleEventPhotos")]
        public async Task<ActionResult> GetCircleEventPhotos(int id)
        {
            return await base.GetPhotos(id);
        }

        [HttpPost("{id}/photo")]
        [Authorize]
        public async Task<ActionResult> AddCircleEventPhotos(int id, List<IFormFile> files)
        {
            return await AddPhotos(id, files, "GetCircleEventPhotos");
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteCircleEventPhoto(int id, int photoId)
        {
            return await DeletePhoto(id, photoId);
        }

        private async Task deleteAttachedPhotos(ICollection<Photo> photos)
        {
            if (photos != null)
            {
                var errors = this._imageFileStorageManager.DeleteAttachedPhotos(photos);
                foreach (var error in errors)
                {
                    this._logger.LogError(error);
                }
                await _repo.SaveAll();
            }
        }
    }
}