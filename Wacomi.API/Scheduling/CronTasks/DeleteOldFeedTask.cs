using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Helper;

namespace Wacomi.API.Scheduling.CronTasks
{
    public class DeleteOldFeedTask : IScheduledTask
    {
        public string Schedule => "0 0 * * *";

        private readonly IBlogRepository _blogRepo;
        private readonly ILogger<DeleteOldFeedTask> _logger;
        private readonly ImageFileStorageManager _fileStorageManager;

        public DeleteOldFeedTask(IServiceProvider serviceProvider, ILogger<DeleteOldFeedTask> logger, ImageFileStorageManager fileStorageManager)
        {
            this._logger = logger;
            this._fileStorageManager = fileStorageManager;

            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._blogRepo = scope.ServiceProvider.GetService<IBlogRepository>();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron Task - Delete Old Feeds");
            _logger.LogInformation("Cron Task - Delete Old Feeds");

            var targetDate = DateTime.Now.AddMonths(-6);
          //  var targetDate = DateTime.Now; //for test

            var deletingFeeds = await _blogRepo.GetBlogFeeds(null, targetDate);
            foreach(var feed in deletingFeeds){
                await this._blogRepo.DeleteFeed(feed);
                this._fileStorageManager.DeleteImageFile(feed.Photo);
                await this._blogRepo.SaveAll();
            }

            Console.WriteLine("Cron Task - Finished DeleteOldFeedTask");
            _logger.LogInformation("Cron Task - Finished DeleteOldFeedTask");
        }
    }
}