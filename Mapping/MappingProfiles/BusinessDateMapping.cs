using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Extensions.DateTime;
using vegaplannerserver.Controllers.Resources;
using vegaplannerserver.Core.Models.Settings;

namespace vegaplannerserver.Mapping.MappingProfiles
{
    public class BusinessDateMapping : Profile
    {
        public BusinessDateMapping()
        { 
        CreateMap<BusinessDate, BusinessDateResource>()
                .ForMember(psr => psr.PrevBusDate,
                    opt => opt.MapFrom(ps => ps.PrevBusDate.SettingDateFormat()))
                .ForMember(psr => psr.NextBusDate,
                    opt => opt.MapFrom(ps => ps.PrevBusDate.SettingDateFormat()))
                .ForMember(psr => psr.CurrBusDate,
                    opt => opt.MapFrom(ps => ps.CurrBusDate.SettingDateFormat()));
        }
    }
}