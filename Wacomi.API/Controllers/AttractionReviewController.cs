using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class AttractionReviewController : DataController
    {
        private readonly IAttractionRepository _attractionRepo;
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ILogger<AttractionReviewController> _logger;
        public AttractionReviewController(IAppUserRepository appUserRepository, IMapper mapper, ILogger<AttractionReviewController> logger, ImageFileStorageManager imageFileStorageManager, IAttractionRepository attractionRepo) : base(appUserRepository, mapper)
        {
            this._logger = logger;
            this._attractionRepo = attractionRepo;
            this._imageFileStorageManager = imageFileStorageManager;
        }

        [HttpGet("{id}", Name = "GetAttractionReview")]
        public async Task<ActionResult> Get(int id)
        {
            var attractionReviewFromRepo = await _attractionRepo.GetAttractionReview(id);
            return Ok(_mapper.Map<AttractionReviewForReturnDto>(attractionReviewFromRepo));
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatest()
        {
            var attractionReviewsFromRepo = await _attractionRepo.GetLatestAttractionReviews();
            return Ok(_mapper.Map<IEnumerable<AttractionReviewForReturnDto>>(attractionReviewsFromRepo));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]AttractionReviewUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken((int)model.AppUserId))
                return Unauthorized();
            if (!await _attractionRepo.AttractionExists(model.AttractionId))
                return NotFound("Attraction " + model.AttractionId + " is not exist");
            if(await _attractionRepo.AttractionReviewed(model.AppUserId, model.AttractionId))
                return BadRequest("既にレビューされています");

            var newAttractionReview = this._mapper.Map<AttractionReview>(model);
            _attractionRepo.Add(newAttractionReview);
            if (await _attractionRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetAttractionReview", new { id = newAttractionReview.Id }, _mapper.Map<AttractionReviewForReturnDto>(newAttractionReview));
            }
            return BadRequest("Failed to add attraction review");
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]AttractionReviewUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var attractionReviewFromRepo = await this._attractionRepo.GetAttractionReview(model.Id);
            if (attractionReviewFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)attractionReviewFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _mapper.Map(model, attractionReviewFromRepo);

            try
            {
                await _attractionRepo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update attraction review: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var attractionReview = await _attractionRepo.GetAttractionReview(id);
            if (attractionReview == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)attractionReview.AppUserId))
            {
                return Unauthorized();
            }
            _attractionRepo.Delete(attractionReview);
            await _attractionRepo.SaveAll();

            foreach (var photo in attractionReview.Photos)
            {
                _attractionRepo.Delete(photo);
                var deletingResult = this._imageFileStorageManager.DeleteImageFile(photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    this._logger.LogError(deletingResult.Error);
            }

            await _attractionRepo.SaveAll();
            return Ok();
        }

    }
}