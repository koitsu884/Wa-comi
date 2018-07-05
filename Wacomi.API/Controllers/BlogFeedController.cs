using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class BlogFeedController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public BlogFeedController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet()]
        public async Task<ActionResult> GetBlogFeeds()
        {
            var blogFeeds = await _repo.GetLatestBlogFeeds();
            var blogFeedsForReturn = this._mapper.Map<IEnumerable<BlogFeedForReturnDto>>(blogFeeds);
            return Ok(blogFeedsForReturn);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogFeed(int id){
            var feed = await _repo.GetBlogFeed(id);
            if(feed == null){
                return NotFound();
            }
            _repo.Delete(feed);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("Failed to delete the blog feed");
        }
    }
}