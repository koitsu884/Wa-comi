using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;

namespace Wacomi.API.Scheduling.CronTasks
{
    public class ChangeTopicTask : IScheduledTask
    {
        public string Schedule => "0 0 * * *";
        private readonly IDailyTopicRepository _repo;
        private readonly ILogger<ChangeTopicTask> _logger;

        public ChangeTopicTask(IServiceProvider serviceProvider, ILogger<ChangeTopicTask> logger)
        {
            this._logger = logger;
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._repo = scope.ServiceProvider.GetService<IDailyTopicRepository>();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
             Console.WriteLine("Cron Task - ChangeTopicTask");
             _logger.LogInformation("Cron Task - ChangeTopicTask");
            var oldTopic = await _repo.GetActiveDailyTopic();
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

            newTopic.IsActive = true;

            _repo.ResetTopicComments();
            await _repo.SaveAll();

            Console.WriteLine("End ChangeTopicTask Old Topic:" + oldTopic.Title + " -> New Topic:" + newTopic.Title);
             _logger.LogInformation("End ChangeTopicTask Old Topic:" + oldTopic.Title + " -> New Topic:" + newTopic.Title);
        }
    }
}