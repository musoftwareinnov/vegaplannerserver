using System;
using System.Threading;
using System.Threading.Tasks;

// namespace Scheduler.Code
// {
//     public class RollBusinessDate : IScheduledTask
//     {
//         public string Schedule => "5 * * * 1-5";

//         public async Task ExecuteAsync(CancellationToken cancellationToken)
//         {
//             Console.WriteLine("Business Date Roll Ran at : " + DateTime.Now);
//             await Task.Delay(5000, cancellationToken);
//         }
//     }
// }