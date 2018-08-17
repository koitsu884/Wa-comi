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
        public string Schedule => "* * * * *";

        private readonly IDataRepository _repo;
        private readonly ILogger<DeleteOldFeedTask> _logger;
        private readonly ImageFileStorageManager _fileStorageManager;

        public DeleteOldFeedTask(IServiceProvider serviceProvider, ILogger<DeleteOldFeedTask> logger, ImageFileStorageManager fileStorageManager)
        {
            this._logger = logger;
            this._fileStorageManager = fileStorageManager;

            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._repo = scope.ServiceProvider.GetService<IDataRepository>();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cron Task - Delete Old Feeds");
            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(DateTime.Now);
            _logger.LogInformation("Cron Task - Delete Old Feeds");

            var targetDate = DateTime.Now.AddMonths(-6);

            var deletingFeeds = await _repo.GetBlogFeeds(null, targetDate);
            foreach(var feed in deletingFeeds){
                await this._repo.DeleteFeed(feed);
                await this._repo.SaveAll();
                this._fileStorageManager.DeleteImageFile(feed.Photo);
            }
        }
    }
}