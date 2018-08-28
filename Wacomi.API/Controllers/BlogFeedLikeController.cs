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
        public BlogFeedLikeController(IDataRepository repo, IMapper mapper) : base(repo, mapper) { }

        [HttpGet("{id}", Name = "GetBlogFeedLike")]
        public async Task<ActionResult> Get(int id)
        {
            var blogFeedLike = await _repo.GetBlogFeedLike(id);
            return Ok(blogFeedLike);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetLikedList(int userId)
        {
            var listFromRepo = await _repo.GetBlogFeedLikesForUser(userId);
            List<int> numberList = new List<int>();
            foreach(var like in listFromRepo){
                numberList.Add((int)like.BlogFeedId);
            }
            return Ok(numberList);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]BlogFeedLike model)
        {
            if(!await MatchAppUserWithToken((int)model.SupportAppUserId))
                return Unauthorized();

            if(!await _repo.RecordExist("AppUser", (int)model.SupportAppUserId))
                return NotFound();

            var blogFeedFromRepo = await _repo.GetBlogFeed((int)model.BlogFeedId);
            if (blogFeedFromRepo == null)
                return NotFound();

            if(await _repo.BlogFeedLiked((int)model.SupportAppUserId, (int)model.BlogFeedId))
                return BadRequest("既にリアクションされています");

            _repo.Add(model);
            await _repo.AddLikeCountToUser(blogFeedFromRepo.Blog.OwnerId);
            if (await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetBlogFeedLike", new {id = model.Id}, null);
            }
            return BadRequest("Failed to post topic like");
        }
    }
}