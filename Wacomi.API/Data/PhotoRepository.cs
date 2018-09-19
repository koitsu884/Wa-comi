using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class PhotoRepository : RepositoryBase, IPhotoRepository
    {
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ILogger<PhotoRepository> _logger;
        public PhotoRepository(ApplicationDbContext context, ILogger<PhotoRepository> logger, ImageFileStorageManager imageFileStorageManager) : base(context)
        {
            this._logger = logger;
            this._imageFileStorageManager = imageFileStorageManager;
        }
        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Photo> GetPhotoByPublicId(string publicId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<IEnumerable<Photo>> GetPhotosForDbSet(string dbsetName, int recordId)
        {
            PropertyInfo entityProperty = _context.GetType().GetProperty(dbsetName);
            if(entityProperty == null)
                return null;
            
            var record = await ((IQueryable<IDataItemWithMultiplePhotos>)entityProperty.GetValue(_context, null)).Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == recordId);
            if (record != null)
                return record.Photos.ToList();
            return null;
        }

        public async Task<Photo> GetPhotoForDbSet(string dbsetName, int recordId)
        {
            PropertyInfo entityProperty = _context.GetType().GetProperty(dbsetName);
            if(entityProperty == null)
                return null;
            
            var record = await ((IQueryable<IDataItemWithSinglePhoto>)entityProperty.GetValue(_context, null)).Include(r => r.Photo).FirstOrDefaultAsync(r => r.Id == recordId);
            if (record != null)
                return record.Photo;
            return null;
        }

        // public async Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId)
        // {
        //     switch (recordType.ToLower())
        //     {
        //         case "appuser":
        //             var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
        //             if (appUser != null)
        //             {
        //                 return appUser.Photos.ToList();
        //             }
        //             break;
        //         case "attraction":
        //             var attraction = await _context.Attractions.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
        //             if (attraction != null)
        //             {
        //                 return attraction.Photos.ToList();
        //             }
        //             break;
        //         case "attractionreview":
        //             var attractionReview = await _context.AttractionReviews.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
        //             if (attractionReview != null)
        //             {
        //                 return attractionReview.Photos.ToList();
        //             }
        //             break;
        //         case "clanseek":
        //             var clanSeek = await _context.ClanSeeks.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
        //             if (clanSeek != null)
        //             {
        //                 return clanSeek.Photos.ToList();
        //             }
        //             break;
        //         case "property":
        //             var properties = await _context.Properties.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
        //             if (properties != null)
        //             {
        //                 return properties.Photos.ToList();
        //             }
        //             break;
        //     }
        //     return null;
        // }
        public async Task<IEnumerable<Photo>> GetPhotosForAppUser(int id)
        {
            var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
            if (appUser != null)
            {
                return appUser.Photos.ToList();
            }
            return null;
        }

        public async Task<int> AddPhotosToRecord(IDataItemWithMultiplePhotos record, List<IFormFile> files)
        {
            List<string> errors = new List<string>();
            var addingPhotos = this.savePhotosToStorage(record, files, true, true);

            if (addingPhotos.Count == 0)
            {
                return 0;
            }

            foreach (var photo in addingPhotos)
            {
                record.Photos.Add(photo);
            }
            await SaveAll();
            if (record.MainPhotoId == null)
            {
                record.MainPhotoId = addingPhotos[0].Id;
                await SaveAll();
            }
            return addingPhotos.Count();
        }

        public async Task<int> AddPhotoToRecord(IDataItemWithSinglePhoto record, List<IFormFile> files)
        {
            if(files.Count() > 1){
                _logger.LogError("This record can have only one photo");
                return 0;
            }
            List<string> errors = new List<string>();
            var addingPhotos = this.savePhotosToStorage(record, files, true, true);

            if (addingPhotos.Count == 0)
            {
                return 0;
            }

            //Delete current photo
            if (record.Photo != null)
            {
                var deletingResult = this._imageFileStorageManager.DeleteImageFile(record.Photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    this._logger.LogError(deletingResult.Error);
                Delete(record.Photo);
                await SaveAll();
            }

            record.Photo = addingPhotos[0];     
            await SaveAll();
            return addingPhotos.Count();
        }

        public async Task DeletePhoto(int id){
            var photo = await GetPhoto(id);
            if(photo == null)
                return;
            Delete(photo);
            var deletingResult = this._imageFileStorageManager.DeleteImageFile(photo);
            if (!string.IsNullOrEmpty(deletingResult.Error))
                this._logger.LogError(deletingResult.Error);
            await SaveAll();
        }

        public async Task DeletePhotos(ICollection<Photo> photos){
            foreach (var photo in photos)
            {
                Delete(photo);
                var deletingResult = this._imageFileStorageManager.DeleteImageFile(photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    this._logger.LogError(deletingResult.Error);
            }
            await SaveAll();
        }

        private List<Photo> savePhotosToStorage(IStructuredDataItem record, List<IFormFile> files, bool addThumbnail = false, bool addIcon = false)
        {
            List<Photo> addingPhotos = new List<Photo>();
            string recordType = record.GetType().Name;
            foreach (var file in files)
            {
                var result = this._imageFileStorageManager.SaveImage(recordType, record.Id, file, System.IO.Path.Combine("images", recordType.ToLower()));
                if (!string.IsNullOrEmpty(result.Error))
                {
                    _logger.LogError(result.Error);
                }
                else
                {
                    string ThumbnailUrl = null;
                    string IconUrl = null;
                    if (addThumbnail)
                    {
                        var thumbnailResult = this._imageFileStorageManager.SaveImage(recordType, record.Id, file, System.IO.Path.Combine("images", recordType, "thumbs"), 300, true);
                        if (string.IsNullOrEmpty(thumbnailResult.Error))
                        {
                            ThumbnailUrl = thumbnailResult.Url;
                        }
                    }
                    if (addIcon)
                    {
                        var iconResult = this._imageFileStorageManager.SaveImage(recordType, record.Id, file, System.IO.Path.Combine("images", recordType.ToLower(), "icons"), 100, true);
                        if (string.IsNullOrEmpty(iconResult.Error))
                            IconUrl = iconResult.Url;
                    }

                    addingPhotos.Add(new Photo()
                    {
                        StorageType = this._imageFileStorageManager.GetStorageType(recordType),
                        Url = result.Url,
                        ThumbnailUrl = ThumbnailUrl,
                        IconUrl = IconUrl,
                        PublicId = result.PublicId,
                    });
                }
            }
            return addingPhotos;
        }

    }
}