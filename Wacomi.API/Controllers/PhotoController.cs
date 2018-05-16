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
    public class PhotoController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        //Cloudinary Specific
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotoController(IDataRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            this._cloudinaryConfig = cloudinarySettings;
            this._mapper = mapper;
            this._repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpGet("{type}/{recordId}")]
        public async Task<ActionResult> GetPhotosForClass(string type, int recordId)
        {
            if(!await this.MatchUserWithToken(type, recordId))
                return Unauthorized();
            var photosFromRepo = await _repo.GetPhotosForClass(type, recordId);
            var photosForReturn = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photosFromRepo);

            return Ok(photosForReturn);
        }

        [HttpPost("{type}/{recordId}")]
        public async Task<ActionResult> AddPhotoForClass(string type, int recordId, [FromForm]PhotoForCreationDto photoForCreationDto){
            
            try
            {
                //Just for validation
                using (var image = System.Drawing.Image.FromStream(photoForCreationDto.File.OpenReadStream()))
                {
                    type = type.ToLower();
                    if(!await this.MatchUserWithToken(type, recordId))
                        return Unauthorized();

                    switch(type){
                        case "member":
                            var currentMember = await _repo.GetMember(recordId);
                            return await AddPhotoToUser(currentMember, photoForCreationDto);
                        case "business":
                            var currentBisUser = await _repo.GetBusinessUser(recordId);
                            return await AddPhotoToUser(currentBisUser, photoForCreationDto);
                        default:
                            return BadRequest("Invalid type name '" + type + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest("画像ファイルを正常に読み込めませんでした" + ex.Message);
            }
        }

        [HttpDelete("{type}/{recordId}/{id}")]
        public async Task<ActionResult> DeletePhotoForClass(string type, int recordId, int id){
            type = type.ToLower();
            if(!await this.MatchUserWithToken(type, recordId))
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

            switch(type){
                case "member":
                    var member = await _repo.GetMember(recordId);
                    if(member != null && member.MainPhotoUrl == photoFromRepo.Url){
                        member.MainPhotoUrl = null;
                    }
                    break;
                case "business":
                    var business = await _repo.GetBusinessUser(recordId);
                    if(business != null && business.MainPhotoUrl == photoFromRepo.Url){
                        business.MainPhotoUrl = null;
                    }
                    break;
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }

        private async Task<ActionResult> AddPhotoToUser(UserBase user, PhotoForCreationDto photoDto){
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(currentUserId != user.IdentityId)
                return Unauthorized();

            var file = photoDto.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0){
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoDto);
            user.Photos.Add(photo);
            if(user.MainPhotoUrl == null){
                user.MainPhotoUrl = photoDto.Url;
            }

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        private async Task<bool> MatchUserWithToken(string userType, int userId){
            userType = userType.ToLower();
            string appUserId = "";
            switch(userType){
                case "member":
                    var member = await _repo.GetMember(userId);
                    appUserId = member.IdentityId;
                    break;
                case "business":
                    var bisUser = await _repo.GetBusinessUser(userId);
                    appUserId = bisUser.IdentityId;
                    break;
                default:
                    return false;
            }

            if(appUserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;

            return false;
        }
    }
}