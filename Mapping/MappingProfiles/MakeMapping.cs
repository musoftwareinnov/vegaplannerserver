using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class MakeMapping : Profile
    {
            public MakeMapping()
            {
                CreateMap<Make, MakeResource>();
                CreateMap<Make, KeyValuePairResource>();
                CreateMap<Model, KeyValuePairResource>();
                CreateMap<Model, ModelResource>();
                CreateMap<Feature, KeyValuePairResource>();
            }
    }
}