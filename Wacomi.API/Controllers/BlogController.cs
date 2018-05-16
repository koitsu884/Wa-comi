using System.Security.Claims;
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
    public class BlogController : Controller
    {
        private readonly IDataRepository _repo;
        private readonly IMapper _mapper;
        public BlogController(IDataRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet("{id}", Name = "GetBlog")]
        public async Task<ActionResult> GetBlog(int id)
        {
            return Ok(await _repo.GetBlog(id));
        }

        [HttpGet("{type}/{recordId}")]
        public async Task<ActionResult> GetBlogsForClass(string type, int recordId)
        {
            if(!await this.MatchUserWithToken(type, recordId))
                return Unauthorized();
            var blogsFlomRepo = await _repo.GetBlogsForClass(type, recordId);

            return Ok(blogsFlomRepo);
        }

        [HttpPost("{type}/{recordId}")]
        [Authorize]
        public async Task<IActionResult> AddBlogInfoToUser(string type, int recordId){ 
            if(!await this.MatchUserWithToken(type, recordId))
                return Unauthorized();

            var user = await this._repo.GetUser(type, recordId);
            if(user.Blogs.Count > 0 && !user.IsPremium){
                return BadRequest("ブログは1つだけ登録可能です");
            }
            else if (user.Blogs.Count > 5){
                return BadRequest("ブログは５つまで登録可能です");
            }
            var blog = new Blog(){Title="新規ブログ", WriterName = user.Identity.DisplayName};
            user.Blogs.Add(blog);
            if(await _repo.SaveAll())
            {
                return CreatedAtRoute("GetBlog", new {id = blog.Id}, blog);
            }
            return BadRequest("ブログの作成に失敗しました");
        }

        [HttpPut("{type}/{recordId}/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBlogForUser(string type, int recordId, int id, [FromBody]BlogForUpdateDto model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!await this.MatchUserWithToken(type, recordId))
                return Unauthorized();

            var blogFromRepo = await _repo.GetBlog(id);
            if(blogFromRepo == null)
                return NotFound();

            this._mapper.Map(model, blogFromRepo);
            if (await _repo.SaveAll())
                return Ok();
            
            return BadRequest("ブログ情報の更新に失敗しました");
        }

        [HttpDelete("{type}/{recordId}/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlogForUser(string type, int recordId, int id){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!await this.MatchUserWithToken(type, recordId))
                return Unauthorized();

            var blogFromRepo = await _repo.GetBlog(id);
            if(blogFromRepo == null)
                return NotFound();

            _repo.Delete(blogFromRepo);

           if (await _repo.SaveAll())
                return Ok();

            return BadRequest("ブログ情報の削除に失敗しました");
        }

        private async Task<bool> MatchUserWithToken(string userType, int userId){
            string appUserId = "";
            switch(userType.ToLower()){
                case "member":
                    var member = await _repo.GetMember(userId);
                    appUserId = member.IdentityId;
                    break;
                case "business":
                    var bisUser = await _repo.GetBusinessUser(userId);
                    appUserId = bisUser.IdentityId;
                    break;
                default:
                    return false;
            }

            if(appUserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;

            return false;
        }
    }

    
}