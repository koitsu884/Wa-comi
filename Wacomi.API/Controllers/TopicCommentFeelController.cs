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
    public class TopicCommentFeelController : DataController
    {
        public TopicCommentFeelController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{memberId}/{commentId}", Name = "GetCommentFeel")]
        public async Task<ActionResult> Get(int memberId, int commentId)
        {
            var commentFeelFromRepo = await _repo.GetCommentFeel(memberId, commentId);
            return Ok(_mapper.Map<TopicCommentFeelForReturnDto>(commentFeelFromRepo));
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult> Get(int memberId = 0){
            var listFromRepo = await _repo.GetCommentFeels(memberId);
            var listToReturn = _mapper.Map<IEnumerable<TopicCommentFeelForReturnDto>>(listFromRepo);
            return Ok(listToReturn);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicCommentFeel model){ 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var member = await this._repo.GetMemberProfile(model.MemberId);
            if(member == null)
                return NotFound();
            
            var topicComment = await this._repo.GetTopicComment(model.CommentId);
            if(topicComment == null)
                return NotFound();

            var commentFeel = await this._repo.GetCommentFeel(model.MemberId, model.CommentId);

            if(commentFeel != null)
                return BadRequest("既にリアクションされています");

            _repo.Add(model);
            if(await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetCommentFeel", new {memberId = model.MemberId, commentId = model.CommentId}, _mapper.Map<TopicCommentFeelForReturnDto>(model));
            }
            return BadRequest("コメントへの反応を追加できませんでした");
        }

        [HttpDelete("{memberId}/{commentId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int memberId, int commentId){

            var topicCommentFromRepo = await _repo.GetCommentFeel(memberId, commentId);
            if(topicCommentFromRepo == null)
                return NotFound();

            if(!await this.MatchAppUserWithToken(topicCommentFromRepo.Member.AppUserId))
                return Unauthorized();

            _repo.Delete(topicCommentFromRepo);

           if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("コメント反応の削除に失敗しました");
        }
    }
}