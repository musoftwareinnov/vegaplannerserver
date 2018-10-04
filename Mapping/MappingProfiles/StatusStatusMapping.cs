using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Mapping.MappingProfiles
{
    public class StatusStatusMapping : Profile
    {
        public StatusStatusMapping()
        {  
            CreateMap<StateStatusResource, StateStatus>();
            CreateMap<StateStatus, StateStatusResource>(); 
        }
    }
}