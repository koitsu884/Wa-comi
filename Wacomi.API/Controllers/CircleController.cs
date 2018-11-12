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
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class CircleController : DataWithPhotoController
    {
        private readonly ICircleRepository _repo;
        private readonly ILogger<CircleController> _logger;
        private readonly ImageFileStorageManager _imageFileStorageManager;
        public CircleController(IAppUserRepository appUserRepository,
            IMapper mapper,
            ICircleRepository repo,
            IPhotoRepository photoRepo,
            ImageFileStorageManager imageFileStorageManager,
            ILogger<CircleController> logger) : base(appUserRepository, mapper, photoRepo)
        {
            _repo = repo;
            _imageFileStorageManager = imageFileStorageManager;
            _logger = logger;
        }

        protected override string GetTableName()
        {
            return "Circles";
        }

        [HttpGet("{id}", Name = "GetCircle")]
        public async Task<ActionResult> Get(int id)
        {
            var circleForReturn = _mapper.Map<CircleForReturnDto>(await _repo.GetCircle(id));
            var appUser = await GetLoggedInUserAsync();
            if (appUser != null)
            {
                if (await _repo.IsOwner(appUser.Id, circleForReturn.Id))
                {
                    circleForReturn.IsManageable = true;
                    circleForReturn.IsMember = true;
                }
                else if (await _repo.IsMember(appUser.Id, circleForReturn.Id))
                {
                    circleForReturn.IsMember = true;
                }
                else
                {
                    var request = await _repo.GetCircleRequest(appUser.Id, circleForReturn.Id);
                    if (request != null)
                    {
                        if (request.Declined)
                            circleForReturn.IsDeclined = true;
                        else
                            circleForReturn.IsWaitingApproval = true;
                    }
                }
            }

            circleForReturn.TotalMemberCount = await _repo.GetCircleMemberCount(id);
            return Ok(circleForReturn);
        }

        [HttpGet("{id}/topics")]
        public async Task<ActionResult> GetCircleTopics(PaginationParams paginationParams, int id)
        {
            var circleTopicsForReturn = await _repo.GetCircleTopicList(paginationParams, id);
            Response.AddPagination(circleTopicsForReturn.CurrentPage, circleTopicsForReturn.PageSize, circleTopicsForReturn.TotalCount, circleTopicsForReturn.TotalPages);
            return Ok(_mapper.Map<IEnumerable<CircleTopicForReturnDto>>(circleTopicsForReturn));
        }

        [HttpGet("{id}/topics/latest")]
        public async Task<ActionResult> GetLatestCircleTopics(int id)
        {
            var circleTopicsForReturn = await _repo.GetLatestCircleTopicList(id);
            return Ok(_mapper.Map<IEnumerable<CircleTopicForReturnDto>>(circleTopicsForReturn));
        }

        [HttpGet("{id}/events/latest")]
        public async Task<ActionResult> GetLatestCircleEvents(int id)
        {
            var circleEventsForReturn = await _repo.GetLatestCircleEventList(id);
            return Ok(_mapper.Map<IEnumerable<CircleEventForReturnDto>>(circleEventsForReturn));
        }

        [HttpGet("{id}/events/past")]
        public async Task<ActionResult> GetPastCircleEvents(int id)
        {
            var circleEventsForReturn = await _repo.GetPastCircleEventList(id);
            return Ok(_mapper.Map<IEnumerable<CircleEventForReturnDto>>(circleEventsForReturn));
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestCircles()
        {
            return Ok(this._mapper.Map<IEnumerable<CircleForReturnDto>>(await _repo.GetLatestCircles()));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetByUser(PaginationParams paginationParams, int userId)
        {
            var circles = await _repo.GetCirclesByUser(paginationParams, userId);
            Response.AddPagination(circles.CurrentPage, circles.PageSize, circles.TotalCount, circles.TotalPages);
            return Ok(this._mapper.Map<IEnumerable<CircleForReturnDto>>(circles));
        }

        [HttpGet("user/{userId}/own")]
        public async Task<ActionResult> GetOwnedByUser(PaginationParams paginationParams, int userId)
        {
            var circles = await _repo.GetCirclesOwnedByUser(paginationParams, userId);
            Response.AddPagination(circles.CurrentPage, circles.PageSize, circles.TotalCount, circles.TotalPages);
            return Ok(this._mapper.Map<IEnumerable<CircleForReturnDto>>(circles));
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            return Ok(await _repo.GetCircleCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();

            var newCircle = this._mapper.Map<Circle>(model);
            var ownerUser = await _appUserRepo.GetAppUser(model.AppUserId);
            if (ownerUser == null)
            {
                _logger.LogError("User not found: " + model.AppUserId);
                return NotFound("ユーザーが見つかりません");
            }

            newCircle.CircleMemberList = new List<CircleMember>(){
                new CircleMember(){
                    AppUserId = model.AppUserId,
                    Role = CircleRoleEnum.OWNER,
                    CircleId = newCircle.Id
                }
            };

            _repo.Add(newCircle);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircle", new { id = newCircle.Id }, _mapper.Map<CircleForReturnDto>(newCircle));
        }

        [HttpPost("search")]
        public async Task<ActionResult> SearchCircles(PaginationParams paginationParams, [FromBody]CircleSearchParameter searchParams)
        {
            var circles = await this._repo.GetCircles(paginationParams, searchParams);
            var circlesForReturn = this._mapper.Map<IEnumerable<CircleForReturnDto>>(circles);
            foreach (var circleForReturn in circlesForReturn)
            {
                circleForReturn.TotalMemberCount = await _repo.GetCircleMemberCount(circleForReturn.Id);
            }

            Response.AddPagination(circles.CurrentPage, circles.PageSize, circles.TotalCount, circles.TotalPages);
            return Ok(circlesForReturn);
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]CircleUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var circleFromRepo = await this._repo.GetCircle((int)model.Id);
            if (circleFromRepo == null)
            {
                return NotFound();
            }

            if (!await _repo.CheckUpdatePermission(model.AppUserId, (int)model.Id))
            {
                return Unauthorized();
            }

            if(!model.ApprovalRequired && circleFromRepo.ApprovalRequired)
            {
                await _repo.ApproveAll(circleFromRepo.Id);
            }

            _mapper.Map(model, circleFromRepo);

            try
            {
                await _repo.SaveAll();
            }
            catch (System.Exception ex)
            {
                this._logger.LogError("Failed to update circle: " + ex.Message);
                return BadRequest("Failed to update circle: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var circleFromRepo = await _repo.GetCircle(id);
            if (circleFromRepo == null)
            {
                return NotFound();
            }
            var loggedInUser = await GetLoggedInUserAsync();
            if (!await _repo.CheckUpdatePermission(loggedInUser.Id, id))
            {
                return Unauthorized();
            }

            //TODO: Delete All Photos of relating records (and relating records itself as well)

            _repo.Delete(circleFromRepo);
            await _repo.SaveAll();

            var errors = this._imageFileStorageManager.DeleteAttachedPhotos(circleFromRepo.Photos);
            foreach (var error in errors)
            {
                this._logger.LogError(error);
            }
            await _repo.SaveAll();
            return Ok();
        }

        [HttpGet("{id}/photo", Name = "GetCirclePhotos")]
        public async Task<ActionResult> GetCirclePhotos(int id)
        {
            return await base.GetPhotos(id);
        }

        [HttpPost("{id}/photo")]
        [Authorize]
        public async Task<ActionResult> AddCirclePhotos(int id, List<IFormFile> files)
        {
            return await AddPhotos(id, files, "GetCirclePhotos");
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteCirclePhoto(int id, int photoId)
        {
            return await DeletePhoto(id, photoId);
        }

    }

}