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
    public class DailyTopicReplyController : DataController
    {
        public DailyTopicReplyController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{id}", Name = "GetTopicReply")]
        public async Task<IActionResult> Get(int id)
        {
            var replyFromRepo = await _repo.GetTopicReply(id);
            return Ok(_mapper.Map<TopicReplyForReturnDto>(replyFromRepo));
        }

        [HttpGet("topic/{topicCommentId}")]
        public async Task<IActionResult> GetByTopic(int topicCommentId){
            var repliesFromRepo = await _repo.GetTopicRepliesByCommentId(topicCommentId);
           // var repliesForReturn = _mapper.Map<IEnumerable<TopicReplyForReturnDto>>(repliesFromRepo);
            var repliesForReturn = _mapper.Map<IEnumerable<CommentForReturnDto>>(repliesFromRepo);
            return Ok(repliesForReturn);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicReply model){ 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await this._repo.TopicCommentExists(model.TopicCommentId))
                return NotFound("トピックコメントが見つかりませんでした。 ID:"+model.TopicCommentId);

            var member = await this._repo.GetMemberProfile(model.MemberId.GetValueOrDefault());
            if(member == null)
                return NotFound("メンバーが見つかりませんでした。 ID:" + model.MemberId);
            
            model.MainPhotoUrl = member.AppUser.MainPhotoUrl;
            model.DisplayName = member.AppUser.DisplayName;
            
            _repo.Add(model);
            if(await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicReply", new {id = model.Id}, _mapper.Map<TopicReplyForReturnDto>(model));
            }
            return BadRequest("投稿に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id){

            var topicReplyFromRepo = await _repo.GetTopicReply(id);
            if(topicReplyFromRepo == null)
                return NotFound();

            if(!await this.MatchAppUserWithToken(topicReplyFromRepo.Member.AppUserId))
                return Unauthorized();

            _repo.Delete(topicReplyFromRepo);

           if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("削除に失敗しました");
        }
    }
}