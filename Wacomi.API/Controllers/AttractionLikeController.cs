using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/attraction/like")]
    public class AttractionLikeController : DataController
    {
         private readonly IAttractionRepository _repo;
        public AttractionLikeController(IAttractionRepository repo, IAppUserRepository appUserRepository, IMapper mapper) : base(appUserRepository, mapper)
        {
            this._repo = repo;
        }

        [HttpGet("{attractionId}/{userId}", Name = "GetAttractionLike")]
        public async Task<ActionResult> Get(int attractionId, int userId)
        {
            return Ok(await _repo.GetAttractionLike(userId, attractionId));
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]AttractionLike model)
        {
            if (!await MatchAppUserWithToken((int)model.AppUserId))
                return Unauthorized();

            if (!await _repo.RecordExist("AppUser", (int)model.AppUserId))
                return NotFound();

            var attractionFromRepo = await _repo.GetAttraction((int)model.AttractionId);
            if (attractionFromRepo == null)
                return NotFound();

            var likeRecord = await _repo.GetAttractionLike((int)model.AppUserId, (int)model.AttractionId);
            if(await _repo.AttractionLiked((int)model.AppUserId, (int)model.AttractionId))
                return BadRequest("既にリアクションされています");

            _repo.Add(model);
            await _appUserRepo.AddLikeCountToUser((int)attractionFromRepo.AppUserId);
            if (await _repo.SaveAll() > 0)
            {
                return Created("GetAttractionLike", null);
            }
            return BadRequest("Failed to post attraction like");
        }
    }
}