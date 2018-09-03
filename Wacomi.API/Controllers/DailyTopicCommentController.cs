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
        private readonly IDailyTopicRepository _topicRepo;
        public DailyTopicCommentController(IAppUserRepository appUserRepo, IDailyTopicRepository topicRepo, IMapper mapper) : base(appUserRepo, mapper)
        {
            this._topicRepo = topicRepo;

        }

        [HttpGet("{id}", Name = "GetTopicComment")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await _topicRepo.GetTopicComment(id));
        }

        //List for index page (limited information)
        [HttpGet("list")]
        public async Task<ActionResult> GetList()
        {
            var listFromRepo = await _topicRepo.GetLatestTopicCommentList();
            var listToReturn = _mapper.Map<IEnumerable<TopicCommentListForReturnDto>>(listFromRepo);
            return Ok(listToReturn);
        }

        [HttpGet()]
        public async Task<ActionResult> Get()
        {
            var listFromRepo = await _topicRepo.GetTopicComments();
            var listToReturn = _mapper.Map<IEnumerable<TopicCommentForReturnDto>>(listFromRepo);
            return Ok(listToReturn);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicComment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var appUser = await this._appUserRepo.GetAppUser(model.AppUserId.GetValueOrDefault());
            if (appUser == null)
                return NotFound();

            var topicCommentsForMember = await this._topicRepo.GetTopicCommentsForMember(model.AppUserId.GetValueOrDefault());
            if (topicCommentsForMember.Count() > 0)
                return BadRequest("投稿は１日１回のみです");

            model.PhotoId = appUser.MainPhotoId;
            model.DisplayName = appUser.DisplayName;

            _topicRepo.Add(model);
            if (await _topicRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicComment", new { id = model.Id }, model);
            }
            return BadRequest("投稿に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            var topicCommentFromRepo = await _topicRepo.GetTopicComment(id);
            if (topicCommentFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken(topicCommentFromRepo.AppUserId.GetValueOrDefault()))
                return Unauthorized();

            _topicRepo.Delete(topicCommentFromRepo);

            if (await _topicRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("トピック候補の削除に失敗しました");
        }
    }
}