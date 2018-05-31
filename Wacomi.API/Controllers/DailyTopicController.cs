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
    public class DailyTopicController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public DailyTopicController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetDailyTopic")]
        public async Task<ActionResult> GetDailyTopic(int id)
        {
            return Ok(await _repo.GetDailyTopic(id));
        }

        [HttpGet("top")]
        public async Task<ActionResult> GetTopDailyTopic(){
            return Ok( await _repo.GetTopDailyTopic());
        }

        [HttpGet("today")]
        public async Task<ActionResult> GetTodaysTopic(){
            return Ok( await _repo.GetTodaysTopic());
        }

        [HttpGet()]
        public async Task<ActionResult> GetDailyTopicList(string userId = null){
            var topicListFromRepo = await _repo.GetDailyTopicList();
            var topicListForReturn = _mapper.Map<IEnumerable<DailyTopicForReturnDto>>(topicListFromRepo);
            if(userId != null){
                var topicLikesForUser = await _repo.GetTopicLikesForUser(userId);
                foreach( var topicList in topicListForReturn){
                    if(topicLikesForUser.Any(tl => tl.DailyTopicId == topicList.Id)){
                        topicList.IsLiked = true;
                    }
                }
            }
            
            return Ok(topicListForReturn);
        }

        [HttpGet("newtopic")]
        // [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ChangeTodaysTopic(){
            var oldTopic = await _repo.GetActiveDailyTopic();
            if(oldTopic != null)
            {
                if(oldTopic.IsTemporary){
                    _repo.Delete(oldTopic);
                }
                else{
                    _repo.ResetTopicLikes(oldTopic.Id);
                    oldTopic.LastDiscussed = DateTime.Now;
                    oldTopic.IsActive = false;
                }
            }
            await _repo.SaveAll();

            var newTopic = await _repo.GetTopDailyTopic();
            if(newTopic == null){
                newTopic = await _repo.GetOldestDailyTopic();
            }

            newTopic.IsActive = true;

            _repo.ResetTopicComments();
            await _repo.SaveAll();
            return Ok("New Topic " + newTopic.Title);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddDailyTopic([FromBody]DailyTopicCreationDto model){ 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if( await this._repo.GetPostedTopicCountForUser(model.UserId) >= 3)
                return BadRequest("トピック候補の投稿は１ユーザー３つまでです。");
            
            var newTopic = _mapper.Map<DailyTopic>(model);
            _repo.Add(newTopic);
            if(await _repo.SaveAll())
            {
                return CreatedAtRoute("GetDailyTopic", new {id = newTopic.Id}, newTopic);
            }
            return BadRequest("トピック候補の作成に失敗しました");
        }

        [HttpDelete()]
        [Authorize]
        public async Task<IActionResult> DeleteTopic(string userId, int recordId){
            if(!this.MatchUserWithToken(userId))
                return Unauthorized();

            var topicFromRepo = await _repo.GetDailyTopic(recordId);
            if(topicFromRepo == null)
                return NotFound();

            if(topicFromRepo.UserId != userId)
                return Unauthorized();

            _repo.Delete(topicFromRepo);

           if (await _repo.SaveAll())
                return Ok();

            return BadRequest("トピック候補の削除に失敗しました");
        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeTopic([FromBody]TopicLike model){
            if(!this.MatchUserWithToken(model.SupportUserId))
                return Unauthorized();

            var topicFromRepo = await _repo.GetDailyTopic(model.DailyTopicId);
            if(topicFromRepo == null)
                return NotFound();

            if(await _repo.GetTopicLike(model.SupportUserId, model.DailyTopicId) != null){
                return BadRequest("既に良いねされています");
            }

            _repo.Add(new TopicLike(){SupportUserId = model.SupportUserId, DailyTopicId = model.DailyTopicId});
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("トピックいいねに失敗しました…？");
        }

        [HttpDelete("like")]
        [Authorize]
        public async Task<IActionResult> DeleteTopicLike([FromBody]TopicLike model){
            if(!this.MatchUserWithToken(model.SupportUserId))
                return Unauthorized();

            var topicLikeFromRepo = await _repo.GetTopicLike(model.SupportUserId, model.DailyTopicId);
            if(topicLikeFromRepo == null)
                return NotFound();

            if(topicLikeFromRepo.SupportUserId != model.SupportUserId)
                return Unauthorized();

            _repo.Delete(topicLikeFromRepo);

           if (await _repo.SaveAll())
                return Ok();

            return BadRequest("トピックいいねの削除に失敗しました");
        }
        

        private bool MatchUserWithToken(string userId){
            return (userId == User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }

    
}