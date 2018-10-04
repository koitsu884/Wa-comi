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
        private readonly ImageFileStorageManager _fileStorageManager;
        private readonly IBlogRepository _blogRepo;
        public BlogFeedController(IAppUserRepository appUserRepo, IBlogRepository blogRepo, IMapper mapper, ImageFileStorageManager fileStorageManager) : base(appUserRepo, mapper)
        {
            this._blogRepo = blogRepo;
            this._fileStorageManager = fileStorageManager;
        }

        [HttpGet("test")]
        public string Test(){
            return "Test";
        }


        [HttpGet("latest")]
        public async Task<ActionResult> GetLatestBlogFeeds()
        {
            var blogFeeds = await _blogRepo.GetLatestBlogFeeds();
            var blogFeedsForReturn = this._mapper.Map<IEnumerable<BlogFeedForReturnDto>>(blogFeeds);
            return Ok(blogFeedsForReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(this._mapper.Map<BlogFeedForReturnDto>(await this._blogRepo.GetBlogFeed(id)));
        }

        [HttpGet()]
        public async Task<ActionResult> Get(PaginationParams paginationParams, string category, int? userId = null)
        {
            var blogFeeds = await _blogRepo.GetBlogFeeds(paginationParams, category, userId);
            var userNameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userNameIdentifier != null)
            {
                var appUser = await _appUserRepo.GetAppUserByAccountId(userNameIdentifier.Value);

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
            var feed = await _blogRepo.GetBlogFeed(id);
            if (feed == null)
            {
                return NotFound();
            }
            await this._blogRepo.DeleteFeed(feed);
            this._fileStorageManager.DeleteImageFile(feed.Photo);

            await _blogRepo.SaveAll();
            return Ok();
        }

        [Authorize]
        [HttpPut("{id}/disable")]
        public async Task<ActionResult> DisableBlogFeed(int id)
        {
            var feed = await _blogRepo.GetBlogFeed(id);
            if (feed == null)
            {
                return NotFound();
            }
            if (!await this.MatchAppUserWithToken(feed.Blog.OwnerId))
                return Unauthorized();

            feed.IsActive = false;
            if (await _blogRepo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to disable the blog feed");
        }
    }
}