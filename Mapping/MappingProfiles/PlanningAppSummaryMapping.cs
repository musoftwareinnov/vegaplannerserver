using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Extensions.DateTime;

namespace vega.Mapping.MappingProfiles
{
    public class PlanningAppSummaryMapping: Profile
    {  
        public PlanningAppSummaryMapping()
        {
            CreateMap<PlanningApp, PlanningAppSummaryResource>()
                .ForMember(psr => psr.CurrentStateStatus,
                    opt => opt.MapFrom(ps => ps.Current().DynamicStateStatus()))
                .ForMember(psr => psr.CurrentState,
                    opt => opt.MapFrom(ps => ps.Current().state.Name))
                .ForMember(psr => psr.ExpectedStateCompletionDate,
                    opt => opt.MapFrom(ps => ps.Current().DueByDate.SettingDateFormat()))
                .ForMember(psr => psr.NextState,
                    opt => opt.MapFrom(ps => ps.SeekNext().state.Name))
                .ForMember(psr => psr.PlanningStatus, 
                    opt => opt.MapFrom(ps => ps.CurrentPlanningStatus.Name))
                .ForMember(psr => psr.Name, 
                    opt => opt.MapFrom(ps => ps.Name))
                .ForMember(psr => psr.CustomerName,
                    opt => opt.MapFrom(ps => ps.Customer.CustomerContact.FirstName + ' ' + ps.Customer.CustomerContact.LastName))
                .ForMember(psr => psr.CompletionDate, 
                    opt => opt.MapFrom(ps => ps.CompletionDate().SettingDateFormat())); 

        } 
    }
}