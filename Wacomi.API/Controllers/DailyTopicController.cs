using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class DailyTopicController : DataController
    {
        private readonly IDailyTopicRepository _topicRepo;
        public DailyTopicController(IAppUserRepository appUserRepo, IDailyTopicRepository topicRepo, IMapper mapper) : base(appUserRepo, mapper) {
            this._topicRepo = topicRepo;
         }

        [HttpGet("{id}", Name = "GetDailyTopic")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _topicRepo.GetDailyTopic(id));
        }

        [HttpGet("top")]
        public async Task<ActionResult> GetTopDailyTopic()
        {
            return Ok(await _topicRepo.GetTopDailyTopic());
        }

        [HttpGet("today")]
        public async Task<ActionResult> GetTodaysTopic()
        {
            return Ok(await _topicRepo.GetTodaysTopic());
        }

        [HttpGet()]
        public async Task<ActionResult> GetDailyTopicList(int? userId)
        {
            var topicListFromRepo = await _topicRepo.GetDailyTopicList();
            var topicListForReturn = _mapper.Map<IEnumerable<DailyTopicForReturnDto>>(topicListFromRepo);

            return Ok(topicListForReturn);
        }

        [HttpGet("newtopic")]
        // [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ChangeTodaysTopic()
        {
            var oldTopic = await _topicRepo.GetActiveDailyTopic();
            if (oldTopic != null)
            {
                if (oldTopic.IsTemporary)
                {
                    _topicRepo.Delete(oldTopic);
                }
                else
                {
                    _topicRepo.ResetTopicLikes(oldTopic.Id);
                    oldTopic.LastDiscussed = DateTime.Now;
                    oldTopic.IsActive = false;
                }
            }
            await _topicRepo.SaveAll();

            var newTopic = await _topicRepo.GetTopDailyTopic();
            if (newTopic == null)
            {
                newTopic = await _topicRepo.GetOldestDailyTopic();
            }

            newTopic.IsActive = true;

            _topicRepo.ResetTopicComments();
            await _topicRepo.SaveAll();
            return Ok("New Topic " + newTopic.Title);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]DailyTopicCreationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await this._topicRepo.GetPostedTopicCountForUser(model.UserId) >= 3)
                return BadRequest("トピック候補の投稿は１ユーザー３つまでです。");

            var newTopic = _mapper.Map<DailyTopic>(model);
            _topicRepo.Add(newTopic);
            if (await _topicRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetDailyTopic", new { id = newTopic.Id }, newTopic);
            }
            return BadRequest("トピック候補の作成に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var topicFromRepo = await _topicRepo.GetDailyTopic(id);
            if (topicFromRepo == null)
                return NotFound();

            if (!await MatchAppUserWithToken(topicFromRepo.UserId))
                return Unauthorized();

            _topicRepo.Delete(topicFromRepo);

            if (await _topicRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("トピック候補の削除に失敗しました");
        }
    }
}