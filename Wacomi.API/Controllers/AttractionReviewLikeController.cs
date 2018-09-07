using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/attractionreview/like")]
    public class AttractionReviewLikeController : DataController
    {
        private readonly IAttractionRepository _repo;
        public AttractionReviewLikeController(IAttractionRepository repo, IAppUserRepository appUserRepository, IMapper mapper) : base(appUserRepository, mapper)
        {
            this._repo = repo;
        }

         [HttpGet("{reviewId}/{userId}", Name = "GetAttractionReviewLike")]
        public async Task<ActionResult> Get(int reviewId, int userId)
        {
            return Ok(await _repo.GetAttractionReviewLike(userId, reviewId));
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]AttractionReviewLike model)
        {
            if (!await MatchAppUserWithToken((int)model.AppUserId))
                return Unauthorized();

            if (!await _repo.RecordExist("AppUser", (int)model.AppUserId))
                return NotFound();

            var attractionReviewFromRepo = await _repo.GetAttractionReview((int)model.AttractionReviewId);
            if (attractionReviewFromRepo == null)
                return NotFound();

            var likeRecord = await _repo.GetAttractionReviewLike((int)model.AppUserId, (int)model.AttractionReviewId);
            if(likeRecord != null)
                return BadRequest("既にリアクションされています");

            _repo.Add(model);
            await _appUserRepo.AddLikeCountToUser((int)attractionReviewFromRepo.AppUserId);
            if (await _repo.SaveAll() > 0)
            {
                return Created("GetAttractionReviewLike", null);
            }
            return BadRequest("Failed to post attraction review like");
        }
    }
}