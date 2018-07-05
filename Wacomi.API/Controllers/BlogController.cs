using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using CodeHollow.FeedReader;
using CodeHollow.FeedReader.Feeds;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wacomi.API.Data;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : DataController
    {
        private const int MAX_BLOGCOUNT = 1;
        private const int MAX_BLOGCOUNT_PR = 5;

        public BlogController(IDataRepository repo, IMapper mapper) : base(repo, mapper) { }

        [HttpGet("{id}", Name = "GetBlog")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(_mapper.Map<BlogForReturnDto>(await _repo.GetBlog(id)));
        }

        [HttpGet("rss")]
        public async Task<ActionResult> GetRssUrl(string url)
        {
            string feedUrl = null;
            url = url.EndsWith("/") ? url.Substring(0, url.Length - 1) : url;

            if (url.Contains("seesaa.net"))
            {
                feedUrl = url + "/index.rdf";
            }

            if (feedUrl == null)
            {
                HttpClient client = new HttpClient();
                // get answer in non-blocking way
                using (var response = await client.GetAsync(url))
                {
                    using (var content = response.Content)
                    {
                        // read answer in non-blocking way
                        var result = await content.ReadAsStringAsync();
                        var document = new HtmlDocument();
                        document.LoadHtml(result);
                        var links = document.DocumentNode?.SelectNodes("//link");
                        if (links != null){
                            var nodes = links.Where(
                            x => x.Attributes["type"] != null &&
                            (x.Attributes["type"].Value.Contains("application/rss") || x.Attributes["type"].Value.Contains("application/atom")));

                            if(nodes != null)
                                feedUrl = nodes.ElementAt(0).Attributes["href"]?.Value;
                        }

                        // var nodes = document.DocumentNode.SelectNodes("//link[@type='application/rss+xml']");
                        // if (nodes != null)
                        //     feedUrl = nodes[0].GetAttributeValue("href", "");

                        // if (feedUrl == null)
                        // {
                        //     nodes = document.DocumentNode.SelectNodes("//link[@type='application/rss+xhtml']");
                        //     if (nodes != null)
                        //         feedUrl = nodes[0].GetAttributeValue("href", "");
                        // }

                        //Some work with page....
                    }
                }
            }
            return Ok(string.IsNullOrEmpty(feedUrl) ? "RSS Url の自動取得に失敗しました" : feedUrl);
        }


        [HttpGet("user/{appUserId}")]
        public async Task<ActionResult> GetForUser(int appUserId)
        {
            if (!await this.MatchAppUserWithToken(appUserId))
                return Unauthorized();
            var blogsFlomRepo = await _repo.GetBlogsForUser(appUserId);

            return Ok(_mapper.Map<IEnumerable<BlogForReturnDto>>(blogsFlomRepo));
        }

        private async Task<BlogFeed> readRss(Blog blog)
        {
            if (blog.RSS == null || blog.IsActive == false)
                return null;

            var feed = await FeedReader.ReadAsync(blog.RSS);
            if (feed == null)
                return null;

            // string result = "";
            // result += "Feed Title: " + feed.Title + "\n";
            // result += "Feed Description: " + feed.Description + "\n";
            // result += "Feed Image: " + feed.ImageUrl + "\n";
            if (feed.Items.Count > 0)
            {
                var latestFeed = await _repo.GetLatestBlogFeed(blog);
                var firstItem = feed.Items.First();

                if (latestFeed == null || latestFeed.PublishingDate < firstItem.PublishingDate)
                {
                    string firstImageUrl = "";
                    if (firstItem.Content != null)
                    {
                        firstImageUrl = Regex.Match(firstItem.Content, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    }
                    var blogFeed = new BlogFeed()
                    {
                        BlogId = blog.Id,
                        Blog = blog,
                        Title = firstItem.Title,
                        ImageUrl = string.IsNullOrEmpty(firstImageUrl) ? null : firstImageUrl,
                        PublishingDate = firstItem.PublishingDate == null ? DateTime.Now : firstItem.PublishingDate,
                        Url = firstItem.Link
                    };
                    return blogFeed;
                }
            }
            return null;
        }

        [HttpPost("rss")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRssFeeds()
        {
            var cronTask = new CronTask(_repo);
            var result = await cronTask.AddRssFeeds();
            return Ok();



            // var blogs = await _repo.GetBlogs();
            // foreach (var blog in blogs)
            // {
            //     //TODO: Count blog feed for the blog
            //     //TODO: Delete feed if it's more than 20
            //     var blogFeed = await readRss(blog);
            //     if (blogFeed != null)
            //     {
            //         this._repo.Add(blogFeed);
            //     }
            // }
            // var cnt = await this._repo.SaveAll();

            // return Ok(cnt);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddBlogInfoToUser([FromBody]Blog model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await this.MatchAppUserWithToken(model.OwnerId))
                return Unauthorized();

            var user = await this._repo.GetAppUser(model.OwnerId);
            if (user == null)
                return NotFound();

            var blogCount = await this._repo.GetBlogCountForUser(user.Id);

            if (blogCount >= MAX_BLOGCOUNT && !user.IsPremium)
            {
                return BadRequest("ブログは" + MAX_BLOGCOUNT + "つだけ登録可能です");
            }
            else if (blogCount >= MAX_BLOGCOUNT_PR)
            {
                return BadRequest("ブログは" + MAX_BLOGCOUNT_PR + "つまで登録可能です");
            }

            _repo.Add(model);
            if (await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetBlog", new { id = model.Id }, _mapper.Map<BlogForReturnDto>(model));
            }
            return BadRequest("ブログの作成に失敗しました");
        }

        [HttpPost("{appUserId}")]
        [Authorize]
        public async Task<IActionResult> AddBlogInfoToUser(int appUserId)
        {
            if (!await this.MatchAppUserWithToken(appUserId))
                return Unauthorized();

            var user = await this._repo.GetAppUser(appUserId);
            if (user == null)
                return NotFound();

            var blogCount = await this._repo.GetBlogCountForUser(user.Id);

            if (blogCount >= MAX_BLOGCOUNT && !user.IsPremium)
            {
                return BadRequest("ブログは" + MAX_BLOGCOUNT + "つだけ登録可能です");
            }
            else if (blogCount >= MAX_BLOGCOUNT_PR)
            {
                return BadRequest("ブログは" + MAX_BLOGCOUNT_PR + "つまで登録可能です");
            }

            var blog = new Blog() { Title = "新規ブログ", WriterName = user.DisplayName, OwnerId = user.Id };
            //blogs.Add(blog);
            _repo.Add(blog);
            if (await _repo.SaveAll() > 0)
            {
                return CreatedAtRoute("GetBlog", new { id = blog.Id }, _mapper.Map<BlogForReturnDto>(blog));
            }
            return BadRequest("ブログの作成に失敗しました");
        }

        [HttpPut("")]
        [Authorize]
        public async Task<IActionResult> UpdateBlogForUser([FromBody]BlogForUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogFromRepo = await _repo.GetBlog(model.Id);
            if (blogFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken(blogFromRepo.OwnerId))
                return Unauthorized();

            this._mapper.Map(model, blogFromRepo);
            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("ブログ情報の更新に失敗しました");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlogForUser(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var blogFromRepo = await _repo.GetBlog(id);
            if (blogFromRepo == null)
                return NotFound();

            if (!await this.MatchAppUserWithToken(blogFromRepo.OwnerId))
                return Unauthorized();

            _repo.Delete(blogFromRepo);

            if (await _repo.SaveAll() > 0)
                return Ok();

            return BadRequest("ブログ情報の削除に失敗しました");
        }
    }


}