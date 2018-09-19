using System;
using System.Collections.Generic;
using System.Security.Claims;
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
    public class ClanSeekController : DataWithPhotoController
    {
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ILogger<ClanSeekController> _logger;
        const int CLANSEEK_MAX = 5;
        private readonly IClanSeekRepository _clanRepo;
        // private readonly IPhotoRepository _photoRepo;
        public ClanSeekController(
            IAppUserRepository appUserRepo,
            IClanSeekRepository clanRepo,
            IMapper mapper,
            IPhotoRepository photoRepo,
            ILogger<ClanSeekController> logger,
            ImageFileStorageManager imageFileStorageManager
        ) : base(appUserRepo, mapper, photoRepo)
        {
            this._clanRepo = clanRepo;
            // this._photoRepo = photoRepo;
            this._imageFileStorageManager = imageFileStorageManager;
            this._logger = logger;
        }

        protected override string GetTableName()
        {
            return "ClanSeeks";
        }

        [HttpGet("{id}", Name = "GetClanSeek")]
        public async Task<ActionResult> GetClanSeek(int id)
        {
            var clanSeekFromRepo = await _clanRepo.GetClanSeek(id);
            var clanSeekForReturn = _mapper.Map<ClanSeekForReturnDto>(clanSeekFromRepo);
            return Ok(clanSeekForReturn);
        }

        [HttpGet("{id}/photo", Name = "GetClanSeekPhotos")]
        public async Task<ActionResult> GetClanSeekPhotos(int id)
        {
            return await base.GetPhotos(id);
        }

        // [HttpGet("{id}/photo")]
        // public async Task<ActionResult> GetPhotos(int id)
        // {
        //     var photos = await _photoRepo.GetPhotosForDbSet("ClanSeeks", id);
        //     return Ok(_mapper.Map<IEnumerable<PhotoForReturnDto>>(photos));
        // }

        [HttpGet()]
        //        public async Task<ActionResult> GetClanSeeks(int? categoryId, int? cityId, bool? latest)
        public async Task<ActionResult> GetClanSeeks(PaginationParams paginationParams, int? categoryId, int? cityId)
        {
            //var clanSeeks = await _repo.GetClanSeeks(categoryId, cityId, latest);
            var clanSeeks = await _clanRepo.GetClanSeeks(paginationParams, categoryId, cityId);
            var clanSeeksForReturn = this._mapper.Map<IEnumerable<ClanSeekForReturnDto>>(clanSeeks);

            Response.AddPagination(clanSeeks.CurrentPage, clanSeeks.PageSize, clanSeeks.TotalCount, clanSeeks.TotalPages);
            return Ok(clanSeeksForReturn);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetByUser(int userId)
        {
            var clanSeeks = await _clanRepo.GetClanSeeksByUser(userId);
            return Ok(this._mapper.Map<IEnumerable<ClanSeekForReturnDto>>(clanSeeks));
        }

        [HttpGet("user/{userId}/count")]
        public async Task<int> GetCountByUser(int userId)
        {
            return await this._clanRepo.GetClanSeeksCountByUser(userId);
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetClanSeekCategories()
        {
            return Ok(await _clanRepo.GetClanSeekCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddClanSeek([FromBody]ClanSeekForCreationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _clanRepo.GetClanSeeksCountByUser(model.AppUserId) >= CLANSEEK_MAX)
                return BadRequest($"仲間募集投稿は1人{CLANSEEK_MAX}つまでです。不要な投稿を削除してから、改めて投稿してください。");

            var newClanSeek = this._mapper.Map<ClanSeek>(model);
            _clanRepo.Add(newClanSeek);
            if (await _clanRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetClanSeek", new { id = newClanSeek.Id }, newClanSeek);
            }
            return BadRequest("Failed to add clanseek");
        }

        [HttpPost("{id}/photo")]
        [Authorize]
        public async Task<ActionResult> AddClanSeekPhotos(int id, List<IFormFile> files){
            return await AddPhotos(id, files, "GetClanSeekPhotos");
        }

        // [HttpPost("{id}/photo")]
        // [Authorize]
        // public async Task<ActionResult> AddPhotos(int id, List<IFormFile> files){
        //     var clanFromRepo = await _clanRepo.GetClanSeek(id);
        //     if(clanFromRepo == null)
        //         return NotFound();
        //     if (!await this.MatchAppUserWithToken((int)clanFromRepo.AppUserId))
        //         return Unauthorized();

        //     var addedPhotoCount = await _photoRepo.AddPhotosToRecord(clanFromRepo, files);

        //     if(addedPhotoCount == 0)
        //         return BadRequest("写真を追加できませんでした");
        //     return Ok(this._mapper.Map<PropertyForReturnDto>(clanFromRepo));
        // }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> UpdateClanSeek([FromBody]ClanSeekUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clanSeekFromRepo = await this._clanRepo.GetClanSeek(model.Id);
            if (clanSeekFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)clanSeekFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, clanSeekFromRepo);
            if (await _clanRepo.SaveAll() > 0)
            {
                return Ok();
            }
            return BadRequest("募集内容の変更に失敗しました。");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClanSeek(int id)
        {
            var clanSeek = await _clanRepo.GetClanSeek(id);
            if (clanSeek == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken(clanSeek.AppUserId))
            {
                return Unauthorized();
            }
            _clanRepo.Delete(clanSeek);
            await _clanRepo.SaveAll();
            await DeletePhotos(clanSeek.Photos);

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteClanSeekPhoto(int id, int photoId){
            return await DeletePhoto(id, photoId);
        }

        // [Authorize]
        // [HttpDelete("{id}/photo/{photoId}")]
        // public async Task<ActionResult> DeletePhoto(int id, int photoId)
        // {
        //     var clanFromRepo = await _clanRepo.GetClanSeek(id);
        //     if (clanFromRepo == null)
        //     {
        //         return NotFound();
        //     }
        //     if (!await MatchAppUserWithToken((int)clanFromRepo.AppUserId))
        //     {
        //         return Unauthorized();
        //     }

        //     await _photoRepo.DeletePhoto(photoId);
        //     return Ok();
        // }

        [Authorize]
        [HttpPut("{id}/{photoId}")]
        public async Task<IActionResult> ChangeMainPhoto(int id, int photoId)
        {
            var clanSeekFromRepo = await _clanRepo.GetClanSeek(id);
            if (clanSeekFromRepo == null)
                return NotFound("The clanseek was not found");

            if (!await _clanRepo.RecordExist("Photo", photoId))
                return NotFound("Photo " + photoId + " was not found");

            if (!await MatchAppUserWithToken((int)clanSeekFromRepo.AppUserId))
                return Unauthorized();

            clanSeekFromRepo.MainPhotoId = photoId;

            return Ok(await _clanRepo.SaveAll());
        }
    }
}