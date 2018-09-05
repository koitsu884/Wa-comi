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
        private readonly IDailyTopicRepository _topicRepo;
        public TopicLikeController(IAppUserRepository appUserRepo, IDailyTopicRepository topicRepo, IMapper mapper) : base(appUserRepo, mapper)
        {
            this._topicRepo = topicRepo;
        }

        [HttpGet("{userId}/{recordId}", Name = "GetTopicLike")]
        public async Task<ActionResult> Get(int userId, int recordId)
        {
            var commentFeelFromRepo = await _topicRepo.GetTopicLike(userId, recordId);
            return Ok(_mapper.Map<TopicLikeForReturnDto>(commentFeelFromRepo));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> Get(int userId)
        {
            var listFromRepo = await _topicRepo.GetTopicLikesForUser(userId);
            List<int> numberList = new List<int>();
            foreach (var like in listFromRepo)
            {
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

            if (!await MatchAppUserWithToken(model.SupportAppUserId))
                return Unauthorized();

            if (!await _topicRepo.RecordExist("AppUser", model.SupportAppUserId))
                return NotFound();

            if (!await _topicRepo.RecordExist("DailyTopic", model.DailyTopicId))
                return NotFound();

            var topicLike = await this._topicRepo.GetTopicLike(model.SupportAppUserId, model.DailyTopicId);
            if (topicLike != null)
                return BadRequest("既にリアクションされています");

            _topicRepo.Add(model);
            if (await _topicRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicLike", new { userId = model.SupportAppUserId, recordId = model.DailyTopicId }, _mapper.Map<TopicLikeForReturnDto>(model));
            }
            return BadRequest("Failed to post topic like");
        }
    }
}