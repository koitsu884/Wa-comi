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
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Scheduling.CronTasks
{
    public class RssReaderTask : IScheduledTask
    {
        //https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer#cron-expressions
        public string Schedule => "* * * * *";

        private readonly IDataRepository _repo;
        private readonly ILogger<RssReaderTask> _logger;
        private readonly IStaticFileManager _staitcFileManager;

        public RssReaderTask(IServiceProvider serviceProvider, ILogger<RssReaderTask> logger, IStaticFileManager staticFileManager)
        {
            this._logger = logger;
            this._staitcFileManager = staticFileManager;

            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._repo = scope.ServiceProvider.GetService<IDataRepository>();

        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron Task - Rss Read");
            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(DateTime.Now);

            _logger.LogInformation("Cron Task - Rss Read");

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

                    this._repo.Add(blogFeed);
                }
                blog.DateRssRead = DateTime.Now;
            }
            await this._repo.SaveAll();
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

        private void saveFeedImage(BlogFeed blogFeed, bool saveLocal = false)
        {
            if (saveLocal)
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
                        }

                        stream.Flush();
                    }
                }
            }
            else{
                
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