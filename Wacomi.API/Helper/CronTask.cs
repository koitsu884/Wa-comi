using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Hangfire;
using Wacomi.API.Data;
using Wacomi.API.Models;
using Microsoft.SyndicationFeed;

namespace Wacomi.API.Helper
{
    public class CronTask
    {
        readonly IDataRepository _repo;
        public CronTask(IDataRepository repo)
        {
            this._repo = repo;
        }

        public void StartRssReader()
        {
            RecurringJob.AddOrUpdate(
                    () => AddRssFeeds(),
                    Cron.Hourly);
        }

        public void StartTopicManager()
        {
            RecurringJob.AddOrUpdate(
                    () => ChangeTopic(),
                    Cron.Daily);
        }

        //Test
        public void RunTopicManagerOnce()
        {
            var jobId = BackgroundJob.Enqueue(
                 () => ChangeTopic());
        }
        public void RunRssReader()
        {
            var jobId = BackgroundJob.Enqueue(
                 () => AddRssFeeds());
        }

        public async Task ChangeTopic()
        {
            Console.WriteLine("Cron: ChangeTopic");
            var oldTopic = await _repo.GetActiveDailyTopic();
            Console.WriteLine("Old Topic: " + oldTopic.Title);
            if (oldTopic != null)
            {
                if (oldTopic.IsTemporary)
                {
                    _repo.Delete(oldTopic);
                }
                else
                {
                    oldTopic.LastDiscussed = DateTime.Now;
                    oldTopic.IsActive = false;
                }
                _repo.ResetTopicLikes(oldTopic.Id);
            }
            await _repo.SaveAll();

            var newTopic = await _repo.GetTopDailyTopic();
            if (newTopic == null)
            {
                newTopic = await _repo.GetOldestDailyTopic();
            }

            Console.WriteLine("New Topic: " + newTopic.Title);

            newTopic.IsActive = true;

            _repo.ResetTopicComments();
            await _repo.SaveAll();
        }
        public async Task<int> AddRssFeeds()
        {
            Console.WriteLine("AddRssFeeds");
            var blogs = await _repo.GetBlogsForRssFeed();
            foreach (var blog in blogs)
            {
                //TODO: Count blog feed for the blog
                //TODO: Delete feed if it's more than 20
                var blogFeed = await readRss(blog);
                if (blogFeed != null)
                {
                    this._repo.Add(blogFeed);
                }
                blog.DateRssRead = DateTime.Now;
            }
            return await this._repo.SaveAll();
        }

        // private async Task<BlogFeed> readRss(Blog blog){
        //     if(blog.RSS == null || blog.IsActive == false)
        //         return null;

        //     var feedReader = new FeedReader();
        //     try{
        //         feedReader.Load(blog.RSS);

        //         return null;
        //     }
        //     catch(Exception ex){
        //         Console.WriteLine(ex.Message);
        //         return null;
        //     }
        // }
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
                    bool jpgOnly = blog.RSS.Contains("yahoo.co.jp");
                    var firstImageUrl = extractFeedImage(feed.Type, firstItem, jpgOnly);

                    var blogFeed = new BlogFeed()
                    {
                        BlogId = blog.Id,
                        Blog = blog,
                        Title = System.Net.WebUtility.HtmlDecode(firstItem.Title),
                        ImageUrl = firstImageUrl,
                        PublishingDate = firstItem.PublishingDate == null ? System.DateTime.Now : firstItem.PublishingDate,
                        Url = firstItem.Link
                    };
                    return blogFeed;
                }
            }
            return null;
        }

        private string extractFeedImage(FeedType feedType, FeedItem feedItem, bool yahoo = false)
        {
            string firstImageUrl = null;
            string search = yahoo ? "img.+?src=[\"'](.+?)[\"']" : "<img.+?src=[\"']([^\"\']*jpe?g).*[\"'].*?>" ;
            // if(feedItem.Content != null){
            //     firstImageUrl = Regex.Match(feedItem.Content, search, RegexOptions.IgnoreCase).Groups[1].Value;
            //     if(!string.IsNullOrEmpty(firstImageUrl))
            //         return firstImageUrl;
            // }

            if (feedItem.SpecificItem != null)
            {
                var element = feedItem.SpecificItem.Element;
                switch (feedType)
                {
                    case FeedType.Rss_1_0:
                    case FeedType.Rss_2_0:
                    case FeedType.Atom:
                        firstImageUrl = Regex.Match(element.ToString(), search, RegexOptions.IgnoreCase).Groups[1].Value;
                        break;
                }
            }

            return firstImageUrl;
        }
    }
}