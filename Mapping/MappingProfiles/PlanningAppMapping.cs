using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Extensions.DateTime;
using vegaplannerserver.Controllers.Resources;

namespace vega.Mapping.MappingProfiles
{
    public class PlanningAppMapping : Profile
    {  
        public PlanningAppMapping()
        { 
            CreateMap<PlanningApp, PlanningAppResource>()
                .ForMember(psr => psr.CurrentStateStatus,
                    opt => opt.MapFrom(ps => ps.Current().DynamicStateStatus()))
                .ForMember(psr => psr.CurrentState,
                    opt => opt.MapFrom(ps => ps.Current().state.Name))
                .ForMember(psr => psr.NextState,
                    opt => opt.MapFrom(ps => ps.SeekNext().state.Name))
                .ForMember(psr => psr.ExpectedStateCompletionDate,
                    opt => opt.MapFrom(ps => ps.Current().DueByDate.SettingDateFormat()))
                .ForMember(psr => psr.MinDueByDate,
                    opt => opt.MapFrom(ps => ps.SeekPrev().DueByDate.SettingDateFormat()))
                .ForMember(psr => psr.PlanningStatus, 
                    opt => opt.MapFrom(ps => ps.CurrentPlanningStatus.Name))
                .ForMember(psr => psr.Name, 
                    opt => opt.MapFrom(ps => ps.Name))
                .ForMember(psr => psr.Generator, 
                    opt => opt.MapFrom(ps => ps.StateInitialiser.Name))
                .ForMember(psr => psr.CompletionDate, 
                    opt => opt.MapFrom(ps => ps.CompletionDate().SettingDateFormat())) 
                .ForMember(psr => psr.Surveyors, 
                    opt => opt.MapFrom(v => v.Surveyors.Select(vf => vf.AppUser.FirstName
                                                                        + ' ' + vf.AppUser.LastName)))
                .ForMember(psr => psr.Drawers, 
                    opt => opt.MapFrom(v => v.Drawers.Select(vf => vf.AppUser.FirstName
                                                                        + ' ' + vf.AppUser.LastName)))
                .ForMember(psr => psr.Admins, 
                    opt => opt.MapFrom(v => v.Admins.Select(vf => vf.AppUser.FirstName
                                                                        + ' ' + vf.AppUser.LastName)))
                .ForMember(psr => psr.PlanningAppFees, 
                    opt => opt.MapFrom(v => v.Fees.Select(vf => new PlanningAppFeesResource { Id = vf.Fee.Id, Name = vf.Fee.Name, Amount = vf.Amount })));

            
            CreateMap<CreatePlanningAppResource, PlanningApp>()
                .ForMember(s => s.Surveyors, opt => opt.Ignore())
                .ForMember(s => s.Drawers, opt => opt.Ignore())
                .ForMember(s => s.Admins, opt => opt.Ignore());

            CreateMap<UpdatePlanningAppResource, PlanningApp>()                
                    .AfterMap((vr, v) => {
                    //Update Fees
                    var updatedFees = v.Fees.ToList();
                    foreach (var f in updatedFees) {
                        f.Amount = vr.planningAppFees.Where(rf => rf.Id == f.FeeId).SingleOrDefault().Amount;
                    }
                });
        }
    }
}