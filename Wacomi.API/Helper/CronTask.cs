using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hangfire;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class CronTask
    {
        readonly IDataRepository _repo;
        public CronTask(IDataRepository repo)
        {
            this._repo = repo;
        }

        public void StartRssReader() {
            RecurringJob.AddOrUpdate(
                    () => AddRssFeeds(),
                    Cron.Hourly);
        }

        public void StartTopicManager() {
            RecurringJob.AddOrUpdate(
                    () => ChangeTopic(),
                    Cron.Daily);
        }

        private async Task<bool> ChangeTopic() {
            Console.WriteLine("Cron: ChangeTopic");
            var oldTopic = await _repo.GetActiveDailyTopic();
            if(oldTopic != null)
            {
                if(oldTopic.IsTemporary){
                    _repo.Delete(oldTopic);
                }
                else{
                    _repo.ResetTopicLikes(oldTopic.Id);
                    oldTopic.LastDiscussed = DateTime.Now;
                    oldTopic.IsActive = false;
                }
            }
            await _repo.SaveAll();

            var newTopic = await _repo.GetTopDailyTopic();
            if(newTopic == null){
                newTopic = await _repo.GetOldestDailyTopic();
            }

            newTopic.IsActive = true;

            _repo.ResetTopicComments();
            return await _repo.SaveAll();
        }
        private async Task<bool> AddRssFeeds(){
            Console.WriteLine("AddRssFeeds");
            var blogs = await _repo.GetBlogs();
            foreach(var blog in blogs){
                //TODO: Count blog feed for the blog
                //TODO: Delete feed if it's more than 20
                var blogFeed = await readRss(blog);
                if(blogFeed != null){
                    this._repo.Add(blogFeed);
                }
            }
            return await this._repo.SaveAll();
        }
        public async Task<BlogFeed> readRss(Blog blog){
            if(blog.RSS == null || blog.IsActive == false)
                return null;

            var feed = await FeedReader.ReadAsync(blog.RSS);
            if(feed == null)
                return null;

            // string result = "";
            // result += "Feed Title: " + feed.Title + "\n";
            // result += "Feed Description: " + feed.Description + "\n";
            // result += "Feed Image: " + feed.ImageUrl + "\n";
            if( feed.Items.Count > 0){
                var latestFeed = await _repo.GetLatestBlogFeed(blog);
                var firstItem = feed.Items.First();

                if(latestFeed == null || latestFeed.PublishingDate < firstItem.PublishingDate)
                {
                    string firstImageUrl = "";
                    if(firstItem.Content != null){
                        firstImageUrl = Regex.Match(firstItem.Content, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    }
                    var blogFeed = new BlogFeed(){
                        BlogId = blog.Id,
                        Blog = blog,
                        Title = firstItem.Title,
                        ImageUrl = string.IsNullOrEmpty(firstImageUrl) ? null : firstImageUrl,
                        PublishingDate = firstItem.PublishingDate == null ? System.DateTime.Now : firstItem.PublishingDate,
                        Url = firstItem.Link 
                    };
                    return blogFeed;
                }
            }
            return null;
        }
    }
}