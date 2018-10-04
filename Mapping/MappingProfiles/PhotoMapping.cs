using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class PhotoMapping : Profile
    {
        public PhotoMapping()
        {
            CreateMap<Photo, PhotoResource>();
        }
    }
}