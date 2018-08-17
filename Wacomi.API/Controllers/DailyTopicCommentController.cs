using System.Collections.Generic;
using System.Linq;
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
    public class DailyTopicCommentController : DataController
    {
        public DailyTopicCommentController(IDataRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{id}", Name = "GetTopicComment")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _repo.GetTopicComment(id));
        }

        //List for index page (limited information)
        [HttpGet("list")]
        public async Task<ActionResult> GetList(){
            var listFromRepo = await _repo.GetLatestTopicCommentList();
            var listToReturn = _mapper.Map<IEnumerable<TopicCommentListForReturnDto>>(listFromRepo);
            return Ok(listToReturn);
        }

        [HttpGet()]
        public async Task<ActionResult> Get(){
            var listFromRepo = await _repo.GetTopicComments();
            var listToReturn = _mapper.Map<IEnumerable<TopicCommentForReturnDto>>(listFromRepo);
            return Ok(listToReturn);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicComment model){ 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var appUser = await this._repo.GetAppUser(model.AppUserId.GetValueOrDefault());
            if(appUser == null)
                return NotFound();
            
            var topicCommentsForMember = await this._repo.GetTopicCommentsForMember(model.AppUserId.GetValueOrDefault());
            if(topicCommentsForMember.Count() > 0)
                return BadRequest("投稿は１日１回のみです");
            
            model.PhotoId = appUser.MainPhotoId;
            model.DisplayName = appUser.DisplayName;
            
            _repo.Add(model);
            if(await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicComment", new {id = model.Id}, model);
            }
            return BadRequest("投稿に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id){

            var topicCommentFromRepo = await _repo.GetTopicComment(id);
            if(topicCommentFromRepo == null)
                return NotFound();

            if(!await this.MatchAppUserWithToken(topicCommentFromRepo.AppUserId.GetValueOrDefault()))
                return Unauthorized();

            _repo.Delete(topicCommentFromRepo);

           if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("トピック候補の削除に失敗しました");
        }
    }
}