using AutoMapper;
using vegaplannerserver.Controllers.Resources;
using vegaplannerserver.Core.Models;

namespace vegaplannerserver.Mapping.MappingProfiles
{
    public class DescriptionOfWorkMapping : Profile
    {
        public DescriptionOfWorkMapping()
        {  
            CreateMap<DescriptionOfWorkResource, DescriptionOfWork>();
            CreateMap<DescriptionOfWork, DescriptionOfWorkResource>(); 
        }
    }
}