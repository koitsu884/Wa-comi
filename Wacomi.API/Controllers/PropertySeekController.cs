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
    public class PropertySeekController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public PropertySeekController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

         [HttpGet("{id}", Name = "GetPropertySeek")]
        public async Task<ActionResult> GetPropertySeek(int id)
        {
            return Ok(await _repo.GetPropertySeek(id));
        }

        // [HttpGet()]
        // [HttpGet("category{categoryId}")]
        // public async Task<ActionResult> GetClanSeeks(int? categoryId)
        // {
        //     var clanSeeks = await _repo.GetClanSeeks(categoryId);
        //     return Ok(clanSeeks);
        // }

        [HttpGet("categories")]
        public async Task<ActionResult> GetClanSeekCategories(){
            return Ok(await _repo.GetClanSeekCategories());
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> AddPropertySeek([FromBody]PropertySeekForCreationDto model){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newPropertySeek = this._mapper.Map<PropertySeek>(model);
            _repo.Add(newPropertySeek);
            if(await _repo.SaveAll() > 0){
                return CreatedAtRoute("GetPropertySeek", new {id = newPropertySeek.Id}, newPropertySeek); 
            }
            return BadRequest("Failed to add property seek");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePropertySeek(int id){
            var clanSeek = await _repo.GetPropertySeek(id);
            if(clanSeek == null){
                return NotFound();
            }
            _repo.Delete(clanSeek);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the property seek");
        }
    }
}