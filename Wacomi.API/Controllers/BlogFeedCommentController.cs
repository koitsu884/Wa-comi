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
    [Route("api/blogfeed/comment")]
    public class BlogFeedCommentController : DataController
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IBlogRepository _blogRepo;
        public BlogFeedCommentController(IAppUserRepository appUserRepo, IBlogRepository blogRepo, IMapper mapper, INotificationRepository notificationRepo) : base(appUserRepo, mapper)
        {
            this._blogRepo = blogRepo;
            this._notificationRepo = notificationRepo;
        }

        [HttpGet("{id}", Name = "GetBlogFeedComment")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(this._mapper.Map<CommentForReturnDto>(await _blogRepo.GetBlogFeedComment(id)));
        }

        [HttpGet()]
        public async Task<IActionResult> GetByTopic(int blogFeedId)
        {
            var feedCommentsFromRepo = await _blogRepo.GetBlogFeedCommentsForFeed(blogFeedId);
            return Ok(this._mapper.Map<IEnumerable<CommentForReturnDto>>(feedCommentsFromRepo));
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]BlogFeedComment model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var appUser = await this._appUserRepo.GetAppUser((int)model.AppUserId);
            if (appUser == null)
                return NotFound();

            var blogFeed = await this._blogRepo.GetBlogFeed((int)model.BlogFeedId);

            if (blogFeed == null)
                return NotFound();

            if (!await MatchAppUserWithToken(appUser.Id))
            {
                return Unauthorized();
            }

            model.DisplayName = appUser.DisplayName;

            _blogRepo.Add(model);
            if (blogFeed.Blog.OwnerId == model.AppUserId)
                await this._notificationRepo.AddNotificationRepliedForFeedComment(blogFeed);
            else
                await this._notificationRepo.AddNotificationNewPostOnFeedComment(appUser.Id, blogFeed);

            if (await _blogRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetBlogFeedComment", new { id = model.Id }, this._mapper.Map<CommentForReturnDto>(model));
            }
            return BadRequest("投稿に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var blogFeedCommentFromRepo = await _blogRepo.GetBlogFeedComment(id);
            if (blogFeedCommentFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken((int)blogFeedCommentFromRepo.AppUserId))
                return Unauthorized();

            _blogRepo.Delete(blogFeedCommentFromRepo);

            if (await _blogRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("コメントの削除に失敗しました");
        }
    }
}