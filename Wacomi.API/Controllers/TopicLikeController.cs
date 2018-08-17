using System.Collections.Generic;
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
    public class TopicLikeController : DataController
    {
        public TopicLikeController(IDataRepository repo, IMapper mapper) : base(repo, mapper) { }

        [HttpGet("{userId}/{recordId}", Name = "GetTopicLike")]
        public async Task<ActionResult> Get(int userId, int recordId)
        {
            var commentFeelFromRepo = await _repo.GetTopicLike(userId, recordId);
            return Ok(_mapper.Map<TopicLikeForReturnDto>(commentFeelFromRepo));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            var listFromRepo = await _repo.GetTopicLikesForUser(userId);
            List<int> numberList = new List<int>();
            foreach(var like in listFromRepo){
                numberList.Add(like.DailyTopicId);
            }
            return Ok(numberList);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicLike model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await MatchAppUserWithToken(model.SupportAppUserId))
                return Unauthorized();

            if(!await _repo.RecordExist("AppUser", model.SupportAppUserId))
                return NotFound();

            if (!await _repo.RecordExist("DailyTopic", model.DailyTopicId))
                return NotFound();

            var topicLike = await this._repo.GetTopicLike(model.SupportAppUserId, model.DailyTopicId);
            if(topicLike != null)
                return BadRequest("既にリアクションされています");

            _repo.Add(model);
            if (await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicLike", new { userId = model.SupportAppUserId, recordId = model.DailyTopicId }, _mapper.Map<TopicLikeForReturnDto>(model));
            }
            return BadRequest("Failed to post topic like");
        }
    }
}