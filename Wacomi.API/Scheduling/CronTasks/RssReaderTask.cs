using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CodeHollow.FeedReader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Scheduling.CronTasks
{
    public class RssReaderTask : IScheduledTask
    {
        //https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer#cron-expressions
        public string Schedule => "*/20 * * * *";
        // private readonly int BLOGFEED_MAX = 20;

        private readonly IBlogRepository _blogRepo;
        private readonly ILogger<RssReaderTask> _logger;
        private readonly ImageFileStorageManager _fileStorageManager;

        public RssReaderTask(IServiceProvider serviceProvider,
         ILogger<RssReaderTask> logger,
          ImageFileStorageManager fileStorageManager)
        {
            this._logger = logger;
            this._fileStorageManager = fileStorageManager;

            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._blogRepo = scope.ServiceProvider.GetService<IBlogRepository>();

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron Task - Rss Read");
            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(DateTime.Now);

            _logger.LogInformation("Cron Task - Rss Read");

            var blogs = await _blogRepo.GetBlogsForRssFeed();
            foreach (var blog in blogs)
            {
                var blogFeed = await readRss(blog);
                if(blogFeed == null)    continue;

                this._blogRepo.Add(blogFeed);
                blog.DateRssRead = DateTime.Now;
                await this._blogRepo.SaveAll();

                // if(await this._repo.GetBlogFeedsCountForBlog(blog.Id) >= this.BLOGFEED_MAX){
                //     await this._repo.DeleteOldestFeed(blog.id);
                // }
                
                if(!string.IsNullOrEmpty(blogFeed.ImageUrl))
                {
                    try
                    {
                        saveFeedImage(blogFeed);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _logger.LogError(ex.Message);
                    }
                }
            }

            Console.WriteLine("End Rss Read");
            _logger.LogInformation("End Rss Read");
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
                var latestFeed = await _blogRepo.GetLatestBlogFeed(blog.Id);
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

        private void saveFeedImage(BlogFeed blogFeed, bool saveLocal = false)
        {
            var targetFolder = Path.Combine("images/blogfeed", blogFeed.BlogId.ToString());
            this._fileStorageManager.MaxWidth = 250;
            var result = this._fileStorageManager.SaveImageFromUrl(
                                        "blogfeed",
                                        blogFeed.ImageUrl,
                                        DateTime.Now.ToString("yyyy_MM_dd_hh_mm") + ".jpg", 
                                        targetFolder
                                        );

            if(result.Error == null){
                blogFeed.Photo = new Photo(){
                    StorageType = this._fileStorageManager.GetStorageType("blogfeed"),
                    Url = result.Url,
                    ThumbnailUrl = result.Url,
                    IconUrl = result.Url,
                    PublicId = result.PublicId,
                };
                _blogRepo.SaveAll();
            }
        }
        private string extractFeedImage(FeedType feedType, FeedItem feedItem, bool yahoo = false)
        {
            string firstImageUrl = null;
            string search = yahoo ? "img.+?src=[\"'](.+?)[\"']" : "<img.+?src=[\"']([^\"\']*jpe?g).*[\"'].*?>";

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