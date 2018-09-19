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
    public class CircleController : DataWithPhotoController
    {
        private readonly ICircleRepository _repo;
        private readonly  ILogger<CircleController> _logger;
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

        protected override string GetTableName(){
            return "Circles";
        }

         [HttpGet("{id}", Name = "GetCircle")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(_mapper.Map<CircleForReturnDto>(await _repo.GetCircle(id)));
        }

        [HttpGet("test")]
        public async Task<ActionResult> Test(){
            _repo.Test();
            return Ok();
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestCircles()
        {
            return Ok(this._mapper.Map<IEnumerable<CircleForReturnDto>>(await _repo.GetLatestCircles()));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetByUser(int userId)
        {
            var circles = await _repo.GetCirclesByUser(userId);
            return Ok(this._mapper.Map<IEnumerable<CircleForReturnDto>>(circles));
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            return Ok(await _repo.GetCircleCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]CircleUpdateDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();

            var newCircle = this._mapper.Map<Circle>(model);
            var ownerUser = _appUserRepo.GetAppUser(model.AppUserId);
            if(ownerUser == null){
                _logger.LogError("User not found: " + model.AppUserId);
                return NotFound("ユーザーが見つかりません");
            }

            newCircle.CircleMemberList = new List<CircleMember>();
            newCircle.CircleMemberList.Add(new CircleMember(){
                    AppUserId = model.AppUserId,
                    Role = CircleRoleEnum.OWNER
                }
            );
            _repo.Add(newCircle);
            await _repo.SaveAll();

            return CreatedAtRoute("GetCircle", new {id = newCircle.Id}, _mapper.Map<CircleForReturnDto>(newCircle));
        }

        [HttpPost("search")]
        public async Task<ActionResult> SearchCircles(PaginationParams paginationParams, [FromBody]CircleSearchParameter searchParams){
            var circles = await this._repo.GetCircles(paginationParams, searchParams);
            var circlesForReturn = this._mapper.Map<IEnumerable<CircleForReturnDto>>(circles);

            Response.AddPagination(circles.CurrentPage, circles.PageSize, circles.TotalCount, circles.TotalPages);
            return Ok(circlesForReturn);
        }

         [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]CircleUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var circleFromRepo = await this._repo.GetCircle(model.Id);
            if (circleFromRepo == null)
            {
                return NotFound();
            }
 
            if (!await _repo.CheckUpdatePermission(model.AppUserId))
            {
                return Unauthorized();
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
            if (!await _repo.CheckUpdatePermission(loggedInUser.Id))
            {
                return Unauthorized();
            }

            //TODO: Delete All Photos of relating records (and relating records itself as well)

            _repo.Delete(circleFromRepo);
            await _repo.SaveAll();

            var errors = this._imageFileStorageManager.DeleteAttachedPhotos(circleFromRepo.Photos);
            foreach(var error in errors){
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
        public async Task<ActionResult> AddCirclePhotos(int id, List<IFormFile> files){
            return await AddPhotos(id, files, "GetCirclePhotos");
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteCirclePhoto(int id, int photoId){
            return await DeletePhoto(id, photoId);
        }

    }
    
}