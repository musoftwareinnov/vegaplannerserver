using vega.Controllers.Resources.Statistics;

namespace vega.Core
{
    public interface IPlanningStatisticsRepository
    {
         PlanningStatistics getPlanningStatistics();
    }
}