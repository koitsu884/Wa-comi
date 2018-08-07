using System;
using System.Threading;
using System.Threading.Tasks;

namespace Wacomi.API.Scheduling
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}