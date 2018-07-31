using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class BlogFeedController : DataController
    {
        public BlogFeedController(IDataRepository repo, IMapper mapper) : base(repo, mapper) { }

        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestBlogFeeds()
        {
            var blogFeeds = await _repo.GetLatestBlogFeeds();
            var blogFeedsForReturn = this._mapper.Map<IEnumerable<BlogFeedForReturnDto>>(blogFeeds);
            return Ok(blogFeedsForReturn);
        }

        [HttpGet()]
        public async Task<ActionResult> Get(PaginationParams paginationParams, string category)
        {
            var blogFeeds = await _repo.GetBlogFeeds(paginationParams, category);
            var userNameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userNameIdentifier != null)
            {
                var appUser = await _repo.GetAppUserByAccountId(userNameIdentifier.Value);

                if (appUser != null)
                {
                    foreach (var tempFeed in blogFeeds)
                    {
                        foreach (var feedLike in tempFeed.FeedLikes)
                        {
                            if ((int)feedLike.SupportAppUserId == appUser.Id)
                            {
                                tempFeed.IsLiked = true;
                                continue;
                            }
                        }
                    }
                }
            }

            var blogFeedsForReturn = this._mapper.Map<IEnumerable<BlogFeedForReturnDto>>(blogFeeds);

            Response.AddPagination(blogFeeds.CurrentPage, blogFeeds.PageSize, blogFeeds.TotalCount, blogFeeds.TotalPages);
            return Ok(blogFeedsForReturn);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogFeed(int id)
        {
            var feed = await _repo.GetBlogFeed(id);
            if (feed == null)
            {
                return NotFound();
            }
            await _repo.DeleteFeedLikes(feed.Id);
            await _repo.DeleteFeedComments(feed.Id);
            _repo.Delete(feed);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the blog feed");
        }

        [Authorize]
        [HttpPut("{id}/disable")]
        public async Task<ActionResult> DisableBlogFeed(int id)
        {
            var feed = await _repo.GetBlogFeed(id);
            if (feed == null)
            {
                return NotFound();
            }
            if (!await this.MatchAppUserWithToken(feed.Blog.OwnerId))
                return Unauthorized();

            feed.IsActive = false;
            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to disable the blog feed");
        }
    }
}