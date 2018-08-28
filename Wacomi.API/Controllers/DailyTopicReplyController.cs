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
        private readonly INotificationRepository _notificationRepo;
        public DailyTopicReplyController(IDataRepository repo, IMapper mapper, INotificationRepository notificationRepo) : base(repo, mapper)
        {
            this._notificationRepo = notificationRepo;

        }

        [HttpGet("{id}", Name = "GetTopicReply")]
        public async Task<IActionResult> Get(int id)
        {
            var replyFromRepo = await _repo.GetTopicReply(id);
            return Ok(_mapper.Map<TopicReplyForReturnDto>(replyFromRepo));
        }

        [HttpGet("topic/{topicCommentId}")]
        public async Task<IActionResult> GetByTopic(int topicCommentId)
        {
            var repliesFromRepo = await _repo.GetTopicRepliesByCommentId(topicCommentId);
            // var repliesForReturn = _mapper.Map<IEnumerable<TopicReplyForReturnDto>>(repliesFromRepo);
            var repliesForReturn = _mapper.Map<IEnumerable<CommentForReturnDto>>(repliesFromRepo);
            return Ok(repliesForReturn);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]TopicReply model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var topicComment = await this._repo.GetTopicComment(model.TopicCommentId);
            if (topicComment == null)
                return NotFound("トピックコメントが見つかりませんでした。 ID:" + model.TopicCommentId);

            var appUser = await this._repo.GetAppUser(model.AppUserId.GetValueOrDefault());
            if (appUser == null)
                return NotFound("メンバーが見つかりませんでした。 ID:" + model.AppUserId);

            model.PhotoId = appUser.MainPhotoId;
            model.DisplayName = appUser.DisplayName;

            _repo.Add(model);
            if (topicComment.AppUserId == model.AppUserId)
                await this._notificationRepo.AddNotificationRepliedForTopicComment(topicComment);
            else
                await this._notificationRepo.AddNotificationNewPostOnTopicComment(appUser.Id, topicComment);

            if (await _notificationRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetTopicReply", new { id = model.Id }, _mapper.Map<TopicReplyForReturnDto>(model));
            }
            return BadRequest("投稿に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            var topicReplyFromRepo = await _repo.GetTopicReply(id);
            if (topicReplyFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken(topicReplyFromRepo.AppUserId.GetValueOrDefault()))
                return Unauthorized();

            _repo.Delete(topicReplyFromRepo);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("削除に失敗しました");
        }
    }
}