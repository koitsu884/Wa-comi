using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/blogfeed/like")]
    public class BlogFeedLikeController : DataController
    {
        private readonly IBlogRepository _blogRepo;
        public BlogFeedLikeController(IAppUserRepository appUserRepo, IBlogRepository blogRepo, IMapper mapper) : base(appUserRepo, mapper) { 
            this._blogRepo = blogRepo;
        }

        [HttpGet("{id}", Name = "GetBlogFeedLike")]
        public async Task<ActionResult> Get(int id)
        {
            var blogFeedLike = await _blogRepo.GetBlogFeedLike(id);
            return Ok(blogFeedLike);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetLikedList(int userId)
        {
            var listFromRepo = await _blogRepo.GetBlogFeedLikesForUser(userId);
            List<int> numberList = new List<int>();
            foreach (var like in listFromRepo)
            {
                numberList.Add((int)like.BlogFeedId);
            }
            return Ok(numberList);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]BlogFeedLike model)
        {
            if (!await MatchAppUserWithToken((int)model.SupportAppUserId))
                return Unauthorized();

            if (!await _blogRepo.RecordExist("AppUser", (int)model.SupportAppUserId))
                return NotFound();

            var blogFeedFromRepo = await _blogRepo.GetBlogFeed((int)model.BlogFeedId);
            if (blogFeedFromRepo == null)
                return NotFound();

            if (await _blogRepo.BlogFeedLiked((int)model.SupportAppUserId, (int)model.BlogFeedId))
                return BadRequest("既にリアクションされています");

            _blogRepo.Add(model);
            await _appUserRepo.AddLikeCountToUser(blogFeedFromRepo.Blog.OwnerId);
            if (await _blogRepo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetBlogFeedLike", new { id = model.Id }, null);
            }
            return BadRequest("Failed to post topic like");
        }
    }
}