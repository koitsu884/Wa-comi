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
    public class PropertyController : DataController
    {
        private readonly IPropertySeekRepository _repo;
        private readonly  ILogger<PropertyController> _logger;
        private readonly ImageFileStorageManager _imageFileStorageManager;
        public PropertyController(IAppUserRepository appUserRepo, 
            IPropertySeekRepository repo, 
            IMapper mapper,
            ILogger<PropertyController> logger,
            ImageFileStorageManager imageFileStorageManager) : base(appUserRepo, mapper)
        {
            this._repo = repo;
            this._imageFileStorageManager = imageFileStorageManager;
            this._logger = logger;
        }

         [HttpGet("{id}", Name = "GetProperty")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(_mapper.Map<PropertyForReturnDto>(await _repo.GetProperty(id)));
        }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestProperties()
        {
            var memberProfile = await GetLoggedInMemberProfileAsync();
            GenderEnum? gender = memberProfile != null ? (GenderEnum?)memberProfile.Gender : null;
            return Ok(this._mapper.Map<IEnumerable<PropertyForReturnDto>>(await _repo.GetLatestProperties(gender)));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetByUser(int userId)
        {
            var properties = await _repo.GetPropertiesByUser(userId);
            return Ok(this._mapper.Map<IEnumerable<PropertyForReturnDto>>(properties));
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories()
        {
            return Ok(await _repo.GetPropertyCategories());
        }
    
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]PropertyUpdateDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await this.MatchAppUserWithToken(model.AppUserId))
                return Unauthorized();

            var newPropertySeek = this._mapper.Map<Property>(model);
            _repo.Add(newPropertySeek);
            if(await _repo.SaveAll() > 0){
                return CreatedAtRoute("GetProperty", new {id = newPropertySeek.Id}, _mapper.Map<PropertyForReturnDto>(newPropertySeek)); 
            }
            return BadRequest("Failed to add property");
        }

        [HttpPost("search")]
        public async Task<ActionResult> SearchProperties(PaginationParams paginationParams, [FromBody]PropertySeekParameters searchParams){
            var memberProfile = await GetLoggedInMemberProfileAsync();
            searchParams.Gender = memberProfile.Gender;
            var properties = await this._repo.GetProperties(paginationParams, searchParams);
            var propertiesForReturn = this._mapper.Map<IEnumerable<PropertyForReturnDto>>(properties);

            Response.AddPagination(properties.CurrentPage, properties.PageSize, properties.TotalCount, properties.TotalPages);
            return Ok(propertiesForReturn);
        }

        [HttpPut()]
        [Authorize]
        public async Task<ActionResult> Put([FromBody]PropertyUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyFromRepo = await this._repo.GetProperty(model.Id);
            if (propertyFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)propertyFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            propertyFromRepo.Categorizations.Clear();
            await _repo.SaveAll();

            _mapper.Map(model, propertyFromRepo);

            try
            {
                await _repo.SaveAll();
            }
            catch (System.Exception ex)
            {
                return BadRequest("Failed to update property: " + ex.Message);
            }
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var propertyFromRepo = await _repo.GetProperty(id);
            if (propertyFromRepo == null)
            {
                return NotFound();
            }
            if (!await MatchAppUserWithToken((int)propertyFromRepo.AppUserId))
            {
                return Unauthorized();
            }

            _repo.Delete(propertyFromRepo);
            await _repo.SaveAll();

            foreach (var photo in propertyFromRepo.Photos)
            {
                _repo.Delete(photo);
                var deletingResult = this._imageFileStorageManager.DeleteImageFile(photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    this._logger.LogError(deletingResult.Error);
            }

            await _repo.SaveAll();
            return Ok();
        }

    }
}