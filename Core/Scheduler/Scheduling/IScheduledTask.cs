using System.Threading;
using System.Threading.Tasks;
using vega.Core;
using vegaplannerserver.Core;

namespace Scheduler.Code
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}