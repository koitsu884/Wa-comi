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
    public class CircleEventController : DataWithPhotoController
    {
        private readonly ICircleRepository _repo;
        private readonly INotificationRepository _notificationRepo;
        private readonly ILogger<CircleEventController> _logger;
        private readonly ImageFileStorageManager _imageFileStorageManager;

        public CircleEventController(ICircleRepository repo, INotificationRepository notificationRepo, ILogger<CircleEventController> logger, IAppUserRepository appUserRepository, IMapper mapper, IPhotoRepository photoRepo) : base(appUserRepository, mapper, photoRepo)
        {
            this._logger = logger;
            this._repo = repo;
            this._notificationRepo = notificationRepo;
        }

        protected override string GetTableName()
        {
            return "CircleEvents";
        }

        [HttpGet("{id}", Name = "GetCircleEvent")]
        public async Task<ActionResult> Get(int id){
            var circleEventForReturn = _mapper.Map<CircleEventForReturnDto>(await _repo.Get<CircleEvent>(id));
            if(circleEventForReturn == null)
                return NotFound();
            //circleEventForReturn.TopicCommentCounts = await _repo.GetCircleTopicCommentCount(id);

            return Ok(circleEventForReturn);
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
            await _repo.SaveAll();
         //   await _notificationRepo.AddNotification(NotificationEnum.NewCircleEventCreated, model.AppUserId, newEvent);
         //   await _repo.SaveAll();

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

           var errors = this._imageFileStorageManager.DeleteAttachedPhotos(circleEventFromRepo.Photos);
            foreach (var error in errors)
            {
                this._logger.LogError(error);
            }
            await _repo.SaveAll();
            return Ok();
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
    }
}