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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wacomi.API.Helper
{
    public class CronTask
    {
        private readonly IDataRepository _repo;
        private readonly ILogger<CronTask> _logger;
        private readonly IStaticFileManager _staitcFileManager;
        public CronTask(IDataRepository repo, ILogger<CronTask> logger, IStaticFileManager staticFileManager)
        {
            this._repo = repo;
            this._logger = logger;
            this._staitcFileManager = staticFileManager;
        }

        public void StartRssReader()
        {
            RecurringJob.AddOrUpdate(
                    () => AddRssFeeds(),
                    Cron.MinuteInterval(30));
        }

        public void StartTopicManager()
        {
            RecurringJob.AddOrUpdate(
                    () => ChangeTopic(),
                    Cron.Daily);
        }

        public void StartOldFeedsChecker()
        {
            RecurringJob.AddOrUpdate(
                () => DeleteOldFeeds(),
                Cron.Daily
            );
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

        public void RunFeedDelete(){
            var jobId = BackgroundJob.Enqueue(
                 () => DeleteAllFeeds());
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
                    var fileName = Path.GetFileName(blogFeed.ImageUrl);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        try
                        {
                            saveFeedImage(blogFeed);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            blogFeed.ImageUrl = null;
                        }
                    }
                    // }

                    this._repo.Add(blogFeed);
                }
                blog.DateRssRead = DateTime.Now;
            }
            return await this._repo.SaveAll();
        }

        public async Task DeleteOldFeeds()
        {
            var targetDate = DateTime.Now.AddMonths(-6);
           await _repo.DeleteFeeds(targetDate);
           await _repo.SaveAll();
        }

        public async Task DeleteAllFeeds(){
           await _repo.DeleteFeeds();
           await _repo.SaveAll();
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
                var latestFeed = await _repo.GetLatestBlogFeed(blog.Id);
                var firstItem = feed.Items.First();

                if (latestFeed == null || latestFeed.PublishingDate < firstItem.PublishingDate)
                {
                    bool jpgOnly = blog.RSS.Contains("yahoo.co.jp");
                    var firstImageUrl = extractFeedImage(feed.Type, firstItem, jpgOnly);
                    DateTime PublishingDate = firstItem.PublishingDate == null ? System.DateTime.Now : (DateTime)firstItem.PublishingDate;

                    var blogFeed = new BlogFeed()
                    {
                        BlogId = blog.Id,
                        Blog = blog,
                        Title = System.Net.WebUtility.HtmlDecode(firstItem.Title),
                        ImageUrl = firstImageUrl,
                        PublishingDate = PublishingDate,
                        Url = firstItem.Link
                    };
                    return blogFeed;
                }
            }
            return null;
        }

        private void saveFeedImage(BlogFeed blogFeed)
        {
            using (WebClient client = new WebClient())
            {
                using (Stream stream = client.OpenRead(blogFeed.ImageUrl))
                {
                    var bitmap = new Bitmap(stream);

                    if (bitmap != null)
                    {
                        int resizeWidth = 400;
                        int resizeHeight = (int)(bitmap.Height * ((double)resizeWidth / (double)bitmap.Width));
                        Bitmap resizeBmp = new Bitmap(bitmap, resizeWidth, resizeHeight);

                        var fileName = Path.Combine("feedimages", blogFeed.BlogId.ToString(), DateTime.Now.ToString("yyyy_MM_dd_hh_mm")) + ".jpg";
                        //may throw exception
                        this._staitcFileManager.SaveImageFile(fileName, resizeBmp, ImageFormat.Jpeg);
                        blogFeed.ImageUrl = fileName;

                        // var targetFileName = Path.Combine(feedImageFolderPath, blog.Id.ToString(), fileName);
                        // var filePath = Path.Combine("feedimages", fileName);

                        // var directory = Path.GetDirectoryName(physicalFilePath);
                        // Directory.CreateDirectory(directory);
                        // resizeBmp.Save(physicalFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    stream.Flush();
                }
            }
        }
        private string extractFeedImage(FeedType feedType, FeedItem feedItem, bool yahoo = false)
        {
            string firstImageUrl = null;
            string search = yahoo ? "img.+?src=[\"'](.+?)[\"']" : "<img.+?src=[\"']([^\"\']*jpe?g).*[\"'].*?>";
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