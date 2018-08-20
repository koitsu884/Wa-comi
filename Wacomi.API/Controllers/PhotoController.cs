using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Drawing;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using System;
using AutoMapper.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wacomi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PhotoController : DataController
    {
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ILogger<PhotoController> _logger;
        public PhotoController(IDataRepository repo, IMapper mapper,  ILogger<PhotoController> logger, ImageFileStorageManager imageFileStorageManager) : base(repo, mapper)
        {
            this._imageFileStorageManager = imageFileStorageManager;
            this._logger = logger;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult> Get(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpGet("{recordType}/{recordId}", Name = "GetPhotos")]
        public async Task<ActionResult> GetPhotosForRecord(string recordType, int recordId)
        {
            var photoFromRepo = await _repo.GetPhotosForRecord(recordType, recordId);
            var photos = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photoFromRepo);

            return Ok(photos);
        }


        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetPhotosForClass(int userId)
        {
            if (!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var photosFromRepo = await _repo.GetPhotosForAppUser(userId);
            var photosForReturn = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photosFromRepo);

            return Ok(photosForReturn);
        }

        [HttpPost("{recordType}/{recordId}")]
        public async Task<ActionResult> Post(string recordType, int recordId, List<IFormFile> files)
        {
            if(files == null){
                return BadRequest();
            }

            switch(recordType.ToLower()){
                case "appuser":
                    return await addPhotosToAppUser(recordType, recordId, files);
                case "blog":
                    return await addPhotoToBlog(recordType, recordId, files);
                case "clanseek":
                    return await addPhotosToClanSeek(recordType, recordId, files);
            }
            return BadRequest("Invalid Record Type: " + recordType);
        }

          private async Task<ActionResult> addPhotosToAppUser(string recordType, int recordId, List<IFormFile> files){
            var appUser = await _repo.GetAppUser(recordId);
            if(appUser == null)
                return NotFound();
            if(!await MatchAppUserWithToken(recordId))
                return Unauthorized();
            
            List<string> errors = new List<string>();
            List<Photo> addingPhotos = new List<Photo>();
            foreach(var file in files){
                var result = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower()));
                if (!string.IsNullOrEmpty(result.Error))
                {
                    errors.Add(result.Error);
                }
                else{
                    addingPhotos.Add(new Photo(){
                        StorageType = this._imageFileStorageManager.GetStorageType(recordType),
                        Url = result.Url,
                        PublicId = result.PublicId,
                    });
                }
            }

            if(addingPhotos.Count == 0){
                return BadRequest(errors);
            }

            foreach(var photo in addingPhotos){
                appUser.Photos.Add(photo);
            }
            await _repo.SaveAll();
            if(appUser.MainPhotoId == null){
                appUser.MainPhotoId = addingPhotos[0].Id;
                await _repo.SaveAll();
            }

            return CreatedAtRoute("GetPhotos", new { recordId = recordId, recordType = recordType }, null);
        }

        private async Task<ActionResult> addPhotosToClanSeek(string recordType, int recordId, List<IFormFile> files){
            var clanSeek = await _repo.GetClanSeek( recordId);
            if(clanSeek == null)
                return NotFound();
            if(!await MatchAppUserWithToken(clanSeek.AppUserId))
                return Unauthorized();
            
            List<string> errors = new List<string>();
            List<Photo> addingPhotos = new List<Photo>();
            foreach(var file in files){
                var result = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower()));
                if (!string.IsNullOrEmpty(result.Error))
                {
                    errors.Add(result.Error);
                }
                else{
                    addingPhotos.Add(new Photo(){
                        StorageType = this._imageFileStorageManager.GetStorageType(recordType),
                        Url = result.Url,
                        PublicId = result.PublicId,
                    });
                }
            }

            if(addingPhotos.Count == 0){
                return BadRequest(errors);
            }

            foreach(var photo in addingPhotos){
                clanSeek.Photos.Add(photo);
            }
            await _repo.SaveAll();
            if(clanSeek.MainPhotoId == null){
                clanSeek.MainPhotoId = addingPhotos[0].Id;
                await _repo.SaveAll();
            }

            return CreatedAtRoute("GetPhotos", new { recordId = recordId, recordType = recordType }, null);
        }

        private async Task<ActionResult> addPhotoToBlog(string recordType, int recordId, List<IFormFile> files){
            var blog = await _repo.GetBlog( recordId);
            if(blog == null)
                return NotFound();
            if(!await MatchAppUserWithToken(blog.OwnerId))
                return Unauthorized();

            //Delete current photo
            if(blog.Photo != null)
            {
                var deletingResult = this._imageFileStorageManager.DeleteImageFile(blog.Photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    this._logger.LogError(deletingResult.Error);
                _repo.Delete(blog.Photo);
                await _repo.SaveAll();
            }
            
            List<string> errors = new List<string>();
            Photo addingPhoto = null;
            var result = this._imageFileStorageManager.SaveImage(recordType, recordId, files[0], System.IO.Path.Combine("images", recordType.ToLower()));
            if (!string.IsNullOrEmpty(result.Error))
            {
                errors.Add(result.Error);
            }
            else{
                addingPhoto = new Photo(){
                    StorageType = this._imageFileStorageManager.GetStorageType(recordType),
                    Url = result.Url,
                    PublicId = result.PublicId,
                };
            }

            if(addingPhoto == null){
                return BadRequest(errors);
            }

            blog.Photo = addingPhoto;

            await _repo.SaveAll();

            return CreatedAtRoute("GetPhotos", new { recordId = recordId, recordType = recordType }, null);
        }

        [HttpDelete("{recordType}/{recordId}/{id}")]
        public async Task<ActionResult> Delete(string recordType, int recordId, int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            if (photoFromRepo == null)
                return NotFound();

            switch (recordType)
            {
                case "AppUser":
                    return await this.DeleteAppUserPhoto(recordId, photoFromRepo);
                case "Blog":
                    return await this.DeleteBlogPhoto(recordId, photoFromRepo);
                case "ClanSeek":
                    return await this.DeleteClanSeekPhoto(recordId, photoFromRepo);
            }
            return BadRequest("Unknown Record Type:" + recordType);
        }

        private async Task<ActionResult> DeleteAppUserPhoto(int recordId, Photo photoFromRepo)
        {
            var appUser = await _repo.GetAppUser(recordId);
            if (appUser == null)
                return NotFound();
            if (!await MatchAppUserWithToken(appUser.Id))
                return Unauthorized();

            var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(result.Error);
            _repo.Delete(photoFromRepo);
            await _repo.SaveAll();
            return Ok();
        }

        private async Task<ActionResult> DeleteClanSeekPhoto(int recordId, Photo photoFromRepo)
        {
            var clanSeek = await _repo.GetClanSeek(recordId);
            if (clanSeek == null)
                return NotFound();
            if (!await MatchAppUserWithToken(clanSeek.AppUserId))
                return Unauthorized();

            var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(result.Error);
            _repo.Delete(photoFromRepo);
            await _repo.SaveAll();
            return Ok();
        }

        private async Task<ActionResult> DeleteBlogPhoto(int recordId, Photo photoFromRepo)
        {
            var blog = await _repo.GetBlog(recordId);
            if (blog == null)
                return NotFound();
            if (!await MatchAppUserWithToken(blog.OwnerId))
                return Unauthorized();

            var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
            if (!string.IsNullOrEmpty(result.Error))
                return BadRequest(result.Error);
            _repo.Delete(blog.Photo);
            await _repo.SaveAll();
            return Ok();
        }


    }
}