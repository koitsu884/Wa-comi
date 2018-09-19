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
        private readonly IAttractionRepository _attractionRepo;
        private readonly IBlogRepository _blogRepo;
        private readonly IClanSeekRepository _clanSeekRepo;
        private readonly IPhotoRepository _photoRepo;
        private readonly IPropertySeekRepository _propertyRepo;
        private readonly ILogger<PhotoController> _logger;
        public PhotoController(
            IAppUserRepository appUserRepo,
            IAttractionRepository attractionRepo,
            IPropertySeekRepository propertyRepo,
            IBlogRepository blogRepo,
            IClanSeekRepository clanSeekRepo,
            IPhotoRepository photoRepo,
            IMapper mapper,
            ILogger<PhotoController> logger,
            ImageFileStorageManager imageFileStorageManager) : base(appUserRepo, mapper)
        {
            this._imageFileStorageManager = imageFileStorageManager;
            this._attractionRepo = attractionRepo;
            this._blogRepo = blogRepo;
            this._clanSeekRepo = clanSeekRepo;
            this._photoRepo = photoRepo;
            this._propertyRepo = propertyRepo;
            this._logger = logger;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<ActionResult> Get(int id)
        {
            var photoFromRepo = await _photoRepo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        // [HttpGet("{recordType}/{recordId}", Name = "GetPhotosOld")]
        // public async Task<ActionResult> GetPhotosForRecord(string recordType, int recordId)
        // {
        //     IEnumerable<Photo> photsFromRepo;
        //     if (recordType.ToLower() == "circle")
        //         photsFromRepo = await _photoRepo.GetPhotosForDbSet("Circles", recordId);
        //     else
        //         photsFromRepo = await _photoRepo.GetPhotosForRecord(recordType, recordId);
        //     var photos = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photsFromRepo);

        //     return Ok(photos);
        // }


        // [HttpGet("user/{userId}")]
        // public async Task<ActionResult> GetPhotosForClass(int userId)
        // {
        //     if (!await this.MatchAppUserWithToken(userId))
        //         return Unauthorized();
        //     var photosFromRepo = await _photoRepo.GetPhotosForAppUser(userId);
        //     var photosForReturn = _mapper.Map<IEnumerable<PhotoForReturnDto>>(photosFromRepo);

        //     return Ok(photosForReturn);
        // }

        // [HttpPost("{recordType}/{recordId}")]
        // public async Task<ActionResult> Post(string recordType, int recordId, List<IFormFile> files)
        // {
        //     if (files == null)
        //     {
        //         return BadRequest();
        //     }


        //     switch (recordType.ToLower())
        //     {
        //         case "attraction":
        //             return await addPhotosToAttraction(recordType, recordId, files);
        //         case "attractionreview":
        //             return await addPhotosToAttractionReview(recordType, recordId, files);
        //         case "appuser":
        //             return await addPhotosToAppUser(recordType, recordId, files);
        //         case "blog":
        //             return await addPhotoToBlog(recordType, recordId, files);
        //         case "clanseek":
        //             return await addPhotosToClanSeek(recordType, recordId, files);
        //         case "property":
        //             return await addPhotosToProperty(recordType, recordId, files);
        //     }
        //     return BadRequest("Invalid Record Type: " + recordType);
        // }

        // private async Task<ActionResult> addPhotosToRecord(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var record = await _appUserRepo.Get<StructuredDataItem>(recordType, recordId);
        //     if (record == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)record.AppUserId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     var addingPhotos = this.savePhotosToStorage(recordType, recordId, files, true, true);

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     var recordWithSinglePhoto = record as DataItemWithSinglePhoto;
        //     if (recordWithSinglePhoto != null)
        //     {

        //     }

        //     var recordWithMultiplePhoto = record as DataItemWithMultiplePhotos;
        //     if (recordWithMultiplePhoto != null)
        //     {
        //         foreach (var photo in addingPhotos)
        //         {
        //             recordWithMultiplePhoto.Photos.Add(photo);
        //         }
        //         await _appUserRepo.SaveAll();
        //         if (recordWithMultiplePhoto.MainPhotoId == null)
        //         {
        //             recordWithMultiplePhoto.MainPhotoId = addingPhotos[0].Id;
        //             await _appUserRepo.SaveAll();
        //         }
        //         return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        //     }

        //     return BadRequest();

        // }

        // private async Task<ActionResult> addPhotosToProperty(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var property = await _propertyRepo.GetProperty(recordId);
        //     if (property == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)property.AppUserId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     var addingPhotos = this.savePhotosToStorage(recordType, recordId, files, true);

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     foreach (var photo in addingPhotos)
        //     {
        //         property.Photos.Add(photo);
        //     }
        //     await _attractionRepo.SaveAll();
        //     if (property.MainPhotoId == null)
        //     {
        //         property.MainPhotoId = addingPhotos[0].Id;
        //         await _attractionRepo.SaveAll();
        //     }

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }
        // private async Task<ActionResult> addPhotosToAttraction(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var attraction = await _attractionRepo.GetAttraction(recordId);
        //     if (attraction == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)attraction.AppUserId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     var addingPhotos = this.savePhotosToStorage(recordType, recordId, files, true);

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     foreach (var photo in addingPhotos)
        //     {
        //         attraction.Photos.Add(photo);
        //     }
        //     await _attractionRepo.SaveAll();
        //     if (attraction.MainPhotoId == null)
        //     {
        //         attraction.MainPhotoId = addingPhotos[0].Id;
        //         await _attractionRepo.SaveAll();
        //     }

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }

        // private async Task<ActionResult> addPhotosToAttractionReview(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var attractionReview = await _attractionRepo.GetAttractionReview(recordId);
        //     if (attractionReview == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)attractionReview.AppUserId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     var addingPhotos = this.savePhotosToStorage(recordType, recordId, files, true);

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     foreach (var photo in addingPhotos)
        //     {
        //         attractionReview.Photos.Add(photo);
        //     }
        //     await _attractionRepo.SaveAll();
        //     if (attractionReview.MainPhotoId == null)
        //     {
        //         attractionReview.MainPhotoId = addingPhotos[0].Id;
        //         await _attractionRepo.SaveAll();
        //     }

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }

        // private List<Photo> savePhotosToStorage(string recordType, int recordId, List<IFormFile> files, bool addThumbnail = false, bool addIcon = false)
        // {
        //     List<Photo> addingPhotos = new List<Photo>();
        //     foreach (var file in files)
        //     {
        //         var result = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower()));
        //         if (!string.IsNullOrEmpty(result.Error))
        //         {
        //             _logger.LogError(result.Error);
        //         }
        //         else
        //         {
        //             string ThumbnailUrl = null;
        //             string IconUrl = null;
        //             if (addThumbnail)
        //             {
        //                 var thumbnailResult = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower(), "thumbs"), 300, true);
        //                 if (string.IsNullOrEmpty(thumbnailResult.Error))
        //                 {
        //                     ThumbnailUrl = thumbnailResult.Url;
        //                 }
        //             }
        //             if (addIcon)
        //             {
        //                 var iconResult = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower(), "icons"), 100, true);
        //                 if (string.IsNullOrEmpty(iconResult.Error))
        //                     IconUrl = iconResult.Url;
        //             }

        //             addingPhotos.Add(new Photo()
        //             {
        //                 StorageType = this._imageFileStorageManager.GetStorageType(recordType),
        //                 Url = result.Url,
        //                 ThumbnailUrl = ThumbnailUrl,
        //                 IconUrl = IconUrl,
        //                 PublicId = result.PublicId,
        //             });
        //         }
        //     }
        //     return addingPhotos;
        // }

        // private async Task<ActionResult> addPhotosToAppUser(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var appUser = await _appUserRepo.GetAppUser(recordId);
        //     if (appUser == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken(recordId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     List<Photo> addingPhotos = new List<Photo>();
        //     foreach (var file in files)
        //     {
        //         if (!this._imageFileStorageManager.ValidateImage(file))
        //         {
        //             errors.Add("不正な画像ファイルです " + file.FileName);
        //             continue;
        //         }
        //         var result = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower()));
        //         if (!string.IsNullOrEmpty(result.Error))
        //         {
        //             errors.Add(result.Error);
        //         }
        //         else
        //         {
        //             var thumbnailResult = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower(), "thumbs"), 300, true);
        //             var iconResult = this._imageFileStorageManager.SaveImage(recordType, recordId, file, System.IO.Path.Combine("images", recordType.ToLower(), "icons"), 100, true);
        //             addingPhotos.Add(new Photo()
        //             {
        //                 StorageType = this._imageFileStorageManager.GetStorageType(recordType),
        //                 Url = result.Url,
        //                 PublicId = result.PublicId,
        //                 ThumbnailUrl = string.IsNullOrEmpty(thumbnailResult.Error) ? thumbnailResult.Url : null,
        //                 IconUrl = string.IsNullOrEmpty(iconResult.Error) ? iconResult.Url : null,
        //             });
        //         }
        //     }

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     foreach (var photo in addingPhotos)
        //     {
        //         appUser.Photos.Add(photo);
        //     }
        //     await _photoRepo.SaveAll();
        //     if (appUser.MainPhotoId == null)
        //     {
        //         appUser.MainPhotoId = addingPhotos[0].Id;
        //         await _photoRepo.SaveAll();
        //     }

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }

        // private async Task<ActionResult> addPhotosToClanSeek(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var clanSeek = await _clanSeekRepo.GetClanSeek(recordId);
        //     if (clanSeek == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)clanSeek.AppUserId))
        //         return Unauthorized();

        //     List<string> errors = new List<string>();
        //     var addingPhotos = this.savePhotosToStorage(recordType, recordId, files, true);

        //     if (addingPhotos.Count == 0)
        //     {
        //         return BadRequest(errors);
        //     }

        //     foreach (var photo in addingPhotos)
        //     {
        //         clanSeek.Photos.Add(photo);
        //     }
        //     await _photoRepo.SaveAll();
        //     if (clanSeek.MainPhotoId == null)
        //     {
        //         clanSeek.MainPhotoId = addingPhotos[0].Id;
        //         await _photoRepo.SaveAll();
        //     }

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }

        // private async Task<ActionResult> addPhotoToBlog(string recordType, int recordId, List<IFormFile> files)
        // {
        //     var blog = await _blogRepo.GetBlog(recordId);
        //     if (blog == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken(blog.OwnerId))
        //         return Unauthorized();

        //     //Delete current photo
        //     if (blog.Photo != null)
        //     {
        //         var deletingResult = this._imageFileStorageManager.DeleteImageFile(blog.Photo);
        //         if (!string.IsNullOrEmpty(deletingResult.Error))
        //             this._logger.LogError(deletingResult.Error);
        //         _photoRepo.Delete(blog.Photo);
        //         await _photoRepo.SaveAll();
        //     }

        //     List<string> errors = new List<string>();
        //     Photo addingPhoto = null;
        //     var result = this._imageFileStorageManager.SaveImage(recordType, recordId, files[0], System.IO.Path.Combine("images", recordType.ToLower()));
        //     if (!string.IsNullOrEmpty(result.Error))
        //     {
        //         errors.Add(result.Error);
        //     }
        //     else
        //     {
        //         var iconResult = this._imageFileStorageManager.SaveImage(recordType, recordId, files[0], System.IO.Path.Combine("images", recordType.ToLower(), "icons"), 100, true);
        //         addingPhoto = new Photo()
        //         {
        //             StorageType = this._imageFileStorageManager.GetStorageType(recordType),
        //             Url = result.Url,
        //             PublicId = result.PublicId,
        //             IconUrl = string.IsNullOrEmpty(iconResult.Error) ? iconResult.Url : null
        //         };
        //     }

        //     if (addingPhoto == null)
        //     {
        //         return BadRequest(errors);
        //     }

        //     blog.Photo = addingPhoto;

        //     await _photoRepo.SaveAll();

        //     return CreatedAtRoute("GetPhotosOld", new { recordId = recordId, recordType = recordType }, null);
        // }

        // [HttpDelete("{recordType}/{recordId}/{id}")]
        // public async Task<ActionResult> Delete(string recordType, int recordId, int id)
        // {
        //     var photoFromRepo = await _photoRepo.GetPhoto(id);
        //     if (photoFromRepo == null)
        //         return NotFound();

        //     switch (recordType.ToLower())
        //     {
        //         case "attraction":
        //             return await this.DeleteAttractionPhoto(recordId, photoFromRepo);
        //         case "attractionreview":
        //             return await this.DeleteAttractionReviewPhoto(recordId, photoFromRepo);
        //         case "appuser":
        //             return await this.DeleteAppUserPhoto(recordId, photoFromRepo);
        //         case "blog":
        //             return await this.DeleteBlogPhoto(recordId, photoFromRepo);
        //         case "clanseek":
        //             return await this.DeleteClanSeekPhoto(recordId, photoFromRepo);
        //         case "property":
        //             return await this.DeletePropertyPhoto(recordId, photoFromRepo);
        //     }
        //     return BadRequest("Unknown Record Type:" + recordType);
        // }

        // private async Task<ActionResult> DeleteAppUserPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var appUser = await _appUserRepo.GetAppUser(recordId);
        //     if (appUser == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken(appUser.Id))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(photoFromRepo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }

        // private async Task<ActionResult> DeleteAttractionPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var attraction = await _attractionRepo.GetAttraction(recordId);
        //     if (attraction == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)attraction.AppUserId))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(photoFromRepo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }

        // private async Task<ActionResult> DeletePropertyPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var property = await _propertyRepo.GetProperty(recordId);
        //     if (property == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)property.AppUserId))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(photoFromRepo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }

        // private async Task<ActionResult> DeleteAttractionReviewPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var attractionReview = await _attractionRepo.GetAttractionReview(recordId);
        //     if (attractionReview == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)attractionReview.AppUserId))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(photoFromRepo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }

        // private async Task<ActionResult> DeleteClanSeekPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var clanSeek = await _clanSeekRepo.GetClanSeek(recordId);
        //     if (clanSeek == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken((int)clanSeek.AppUserId))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(photoFromRepo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }

        // private async Task<ActionResult> DeleteBlogPhoto(int recordId, Photo photoFromRepo)
        // {
        //     var blog = await _blogRepo.GetBlog(recordId);
        //     if (blog == null)
        //         return NotFound();
        //     if (!await MatchAppUserWithToken(blog.OwnerId))
        //         return Unauthorized();

        //     var result = this._imageFileStorageManager.DeleteImageFile(photoFromRepo);
        //     if (!string.IsNullOrEmpty(result.Error))
        //         return BadRequest(result.Error);
        //     _photoRepo.Delete(blog.Photo);
        //     await _photoRepo.SaveAll();
        //     return Ok();
        // }


    }
}