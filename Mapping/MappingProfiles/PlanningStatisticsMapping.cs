using AutoMapper;
using vega.Controllers.Resources.Statistics;
using vega.Core;

namespace vega.Mapping.MappingProfiles
{
    public class PlanningStatisticsMapping : Profile
    {
        public PlanningStatisticsMapping() 
        {  
            CreateMap<PlanningStatistics, PlanningStatisticsResource>();
        }
    }
}