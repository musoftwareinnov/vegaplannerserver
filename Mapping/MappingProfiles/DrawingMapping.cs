
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class DrawingMapping : Profile
    {
        public DrawingMapping()
        {
            CreateMap<Photo, DrawingResource>();
        }
    }
}