using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using vega.Extensions;
using vega.Core.Models.States;
using Microsoft.Extensions.Options;

namespace vega.Persistence
{
    public class PlanningStatisticsRepository : IPlanningStatisticsRepository
    {
        public PlanningStatisticsRepository(VegaDbContext vegaDbContext, 
                                     IPlanningAppRepository planningAppRepository)
        {
            VegaDbContext = vegaDbContext;
            PlanningAppRepository = planningAppRepository;
        }
        public VegaDbContext VegaDbContext { get; }
        public IPlanningAppRepository PlanningAppRepository { get; }



        public PlanningStatistics getPlanningStatistics() {

            var planingStatistics = new PlanningStatistics();

            PlanningAppQuery planningAppQuery = new PlanningAppQuery();


            QueryResult<PlanningApp> result = new QueryResult<PlanningApp>();

            planningAppQuery.PlanningAppType = StatusList.AppInProgress;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.InProgress = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.OnTime;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.OnTime = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.Due;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.Due = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.Overdue;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.Overdue = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.All;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.All = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.AppTerminated;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.Terminated = result.TotalItems;

            planningAppQuery.PlanningAppType = StatusList.Complete;
            result = PlanningAppRepository.GetPlanningApps(planningAppQuery);
            planingStatistics.Completed = result.TotalItems;

            return planingStatistics;

        }
    }


}
