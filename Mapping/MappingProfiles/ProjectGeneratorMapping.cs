using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Extensions.DateTime;
using vegaplannerserver.Controllers.Resources;

namespace vega.Mapping.MappingProfiles
{
    public class ProjectGeneratorMapping : Profile
    {  
        public ProjectGeneratorMapping()
        { 
            CreateMap<ProjectGenerator, ProjectGeneratorResource>()

                .ForMember(g => g.Generators,
                    opt => opt.MapFrom(g => g.OrderedGenerators));
        }
    }
}