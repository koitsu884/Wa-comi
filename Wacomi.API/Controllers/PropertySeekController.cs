using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class PropertySeekController : DataController
    {
        private readonly IDataRepository _dataRepo;
        public PropertySeekController(IAppUserRepository appUserRepo, IDataRepository dataRepo, IMapper mapper) : base(appUserRepo, mapper)
        {
            this._dataRepo = dataRepo;
        }

         [HttpGet("{id}", Name = "GetPropertySeek")]
        public async Task<ActionResult> GetPropertySeek(int id)
        {
            return Ok(await _dataRepo.GetPropertySeek(id));
        }

        // [HttpGet()]
        // [HttpGet("category{categoryId}")]
        // public async Task<ActionResult> GetClanSeeks(int? categoryId)
        // {
        //     var clanSeeks = await _dataRepo.GetClanSeeks(categoryId);
        //     return Ok(clanSeeks);
        // }

    
        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> AddPropertySeek([FromBody]PropertySeekForCreationDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newPropertySeek = this._mapper.Map<PropertySeek>(model);
            _dataRepo.Add(newPropertySeek);
            if(await _dataRepo.SaveAll() > 0){
                return CreatedAtRoute("GetPropertySeek", new {id = newPropertySeek.Id}, newPropertySeek); 
            }
            return BadRequest("Failed to add property seek");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePropertySeek(int id){
            var clanSeek = await _dataRepo.GetPropertySeek(id);
            if(clanSeek == null){
                return NotFound();
            }
            _dataRepo.Delete(clanSeek);

            if (await _dataRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the property seek");
        }
    }
}