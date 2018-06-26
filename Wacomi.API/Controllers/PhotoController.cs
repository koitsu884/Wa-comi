using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Drawing;
using AutoMapper;
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

namespace Wacomi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PhotoController : DataController
    {
        //Cloudinary Specific
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotoController(IDataRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings) : base(repo, mapper)
        {
            this._cloudinaryConfig = cloudinarySettings;

            CloudinaryDotNet.Account acc = new CloudinaryDotNet.Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult> Get(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetPhotosForClass(int userId)
        {
            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            var photosFromRepo = await _repo.GetPhotosForAppUser(userId);
            var photosForReturn = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photosFromRepo);

            return Ok(photosForReturn);
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> Post(int userId, [FromForm]PhotoForCreationDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = await _repo.GetAppUser(userId);
            if(appUser == null)
                return NotFound();

            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();
            
            try
            {
                //Just for validation
                using (var image = System.Drawing.Image.FromStream(model.File.OpenReadStream()))
                {
                    return await AddPhotoToUser(appUser, model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("画像ファイルを正常に読み込めませんでした" + ex.Message);
            }
        }

        [HttpDelete("{userId}/{id}")]
        public async Task<ActionResult> Delete(int userId, int id){
            var appUser = await _repo.GetAppUser(userId);
            if( appUser == null)
                return NotFound();

            if(!await this.MatchAppUserWithToken(userId))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPhoto(id);
            if(photoFromRepo == null)
                return NotFound();

            if(photoFromRepo.PublicId != null){
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinary.Destroy(deleteParams);
                if(result.Result == "ok" || result.Result =="not found") //Temp fix for not found
                    _repo.Delete(photoFromRepo);
            }

            if(photoFromRepo.PublicId == null){
                _repo.Delete(photoFromRepo);
            }

            if(appUser.MainPhotoUrl == photoFromRepo.Url)
                appUser.MainPhotoUrl = null;

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }

        private async Task<ActionResult> AddPhotoToUser(AppUser user, PhotoForCreationDto photoDto){

            var file = photoDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0){
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(600).Height(600).Crop("fit")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoDto);
            photo.AppUserId = user.Id;
            _repo.Add(photo);
            // if(user.MainPhotoUrl == null){
            //     user.MainPhotoUrl = photoDto.Url;
            // }

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }
    }
}