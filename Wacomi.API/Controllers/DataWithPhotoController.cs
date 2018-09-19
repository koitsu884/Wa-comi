using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    public abstract class DataWithPhotoController : DataController
    {
        private readonly IPhotoRepository _photoRepo;
        private IAppUserRepository appUserRepo;
        private IMapper mapper;

        public DataWithPhotoController(
            IAppUserRepository appUserRepository,
            IMapper mapper,
            IPhotoRepository photoRepo
        ) : base(appUserRepository, mapper)
        {
            this._photoRepo = photoRepo;
        }

        protected abstract string GetTableName();
        protected virtual bool MultiPhoto() { return true; }
        protected async Task DeletePhotos(ICollection<Photo> photos)
        {
            await _photoRepo.DeletePhotos(photos);
        }

        public async Task<ActionResult> GetPhotos(int id)
        {
            if (MultiPhoto())
            {
                var photos = await _photoRepo.GetPhotosForDbSet(GetTableName(), id);
                return Ok(_mapper.Map<IEnumerable<PhotoForReturnDto>>(photos));
            }
            var photo = await _photoRepo.GetPhotoForDbSet(GetTableName(), id);
            return Ok(_mapper.Map<PhotoForReturnDto>(photo));
        }
        public async Task<ActionResult> AddPhotos(int id, List<IFormFile> files, string getMethodName)
        {
            int addedPhotoCount = 0;
            if (MultiPhoto())
            {
                var baseQuery = _photoRepo.GetDataRecordByTableName<IDataItemWithMultiplePhotos>(GetTableName());
                if (baseQuery == null)
                    return BadRequest("Could not get the record " + GetTableName() + " : " + id);
                var recordFromRepo = await baseQuery.Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == id);
                if (recordFromRepo == null)
                    return NotFound();
                var recordWithAppUser = recordFromRepo as IAppUserLinkable;
                if (recordWithAppUser != null && !await this.MatchAppUserWithToken((int)recordWithAppUser.AppUserId))
                    return Unauthorized();
                addedPhotoCount = await _photoRepo.AddPhotosToRecord(recordFromRepo, files);
            }
            else
            {
                var baseQuery = _photoRepo.GetDataRecordByTableName<IDataItemWithSinglePhoto>(GetTableName());
                if (baseQuery == null)
                    return BadRequest("Could not get the record " + GetTableName() + " : " + id);
                var recordFromRepo = await baseQuery.Include(r => r.Photo).FirstOrDefaultAsync(r => r.Id == id);
                if (recordFromRepo == null)
                    return NotFound();
                var recordWithAppUser = recordFromRepo as IAppUserLinkable;
                if (recordWithAppUser != null && !await this.MatchAppUserWithToken((int)recordWithAppUser.AppUserId))
                    return Unauthorized();
                addedPhotoCount = await _photoRepo.AddPhotoToRecord(recordFromRepo, files);
            }


            if (addedPhotoCount == 0)
                return BadRequest("写真を追加できませんでした");

            return CreatedAtRoute(getMethodName, new { id = id }, null);
        }
        public async Task<ActionResult> DeletePhoto(int id, int photoId)
        {
            if (MultiPhoto())
            {
                var baseQuery = _photoRepo.GetDataRecordByTableName<IDataItemWithMultiplePhotos>(GetTableName());
                if (baseQuery == null)
                    return BadRequest("Could not get the record " + GetTableName() + " : " + id);
                var recordFromRepo = await baseQuery.Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == id);
                if (recordFromRepo == null)
                    return NotFound();
                var recordWithAppUser = recordFromRepo as IAppUserLinkable;
                if (recordWithAppUser != null && !await this.MatchAppUserWithToken((int)recordWithAppUser.AppUserId))
                    return Unauthorized();
            }
            else
            {
                var baseQuery = _photoRepo.GetDataRecordByTableName<IDataItemWithSinglePhoto>(GetTableName());
                if (baseQuery == null)
                    return BadRequest("Could not get the record " + GetTableName() + " : " + id);
                var recordFromRepo = await baseQuery.Include(r => r.Photo).FirstOrDefaultAsync(r => r.Id == id);
                if (recordFromRepo == null)
                    return NotFound();
                var recordWithAppUser = recordFromRepo as IAppUserLinkable;
                if (recordWithAppUser != null && !await this.MatchAppUserWithToken((int)recordWithAppUser.AppUserId))
                    return Unauthorized();

            }
            await _photoRepo.DeletePhoto(photoId);
            return Ok();
        }

    }
}