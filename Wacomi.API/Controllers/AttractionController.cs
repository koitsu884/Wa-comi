using System.Collections.Generic;
using System.Linq;
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
    public class AttractionController : DataController
    {
        private readonly IAttractionRepository _attractionRepo;
        private readonly ImageFileStorageManager _imageFileStorageManager;
        private readonly ILogger<AttractionController> _logger;
        public AttractionController(IAppUserRepository appUserRepository,
             IMapper mapper,
             ILogger<AttractionController> logger,
             IAttractionRepository attractionRepo,
             ImageFileStorageManager imageFileStorageManager) : base(appUserRepository, mapper)
        {
            this._logger = logger;
            this._imageFileStorageManager = imageFileStorageManager;
            this._attractionRepo = attractionRepo;
        }

        [HttpGet("{id}", Name = "GetAttraction")]
        public async Task<ActionResult> Get(int id)
        {
            var attractionFromRepo = await _attractionRepo.GetAttraction(id);
            var test = _mapper.Map<AttractionForReturnDto>(attractionFromRepo);
            return Ok(test);
        }

        [HttpGet()]
        public async Task<ActionResult> GetAttractions(string categories, int cityId = 0, int appUserId = 0)
        {
            int[] categoryIdArray = null;
            if (categories != null)
            {
                categoryIdArray = categories.Split(',').Select(System.Int32.Parse).ToArray();
            }
            var attractions = await _attractionRepo.GetAttractions(categoryIdArray, cityId, appUserId);
            // var attractionFromRepo = await _attractionRepo.GetAttraction(id);
            // var test = _mapper.Map<AttractionForReturnDto>(attractionFromRepo);
            return Ok(_mapper.Map<IEnumerable<AttractionForReturnDto>>(attractions));
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestAttractions()
        {
            var attractions = await _attractionRepo.GetLatestAttractions();
            return Ok(_mapper.Map<IEnumerable<AttractionForReturnDto>>(attractions));
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            return Ok(await _attractionRepo.GetAttractionCategories());
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]AttractionUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newAttraction = this._mapper.Map<Attraction>(model);
            // List<AttractionCategorization> categorizationList = new List<AttractionCategorization>();
            // foreach (var categoryId in model.CategoryIds)
            // {
            //     categorizationList.Add(new AttractionCategorization() { AttractionCategoryId = categoryId });
            // }
            // newAttraction.Categorizations = categorizationList;

            _attractionRepo.Add(newAttraction);
            if (await _attractionRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetAttraction", new { id = newAttraction.Id }, _mapper.Map<AttractionForReturnDto>(newAttraction));
            }
            return BadRequest("Failed to add clanseek");
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]AttractionUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var attractionFromRepo = await this._attractionRepo.GetAttraction(model.Id);
            if (attractionFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)attractionFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            attractionFromRepo.Categorizations.Clear();
            await _attractionRepo.SaveAll();

            _mapper.Map(model, attractionFromRepo);

            try
            {
                await _attractionRepo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update attraction: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var attraction = await _attractionRepo.GetAttraction(id);
            if (attraction == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)attraction.AppUserId))
            {
                return Unauthorized();
            }
            _attractionRepo.Delete(attraction);
            await _attractionRepo.SaveAll();

            foreach (var photo in attraction.Photos)
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