using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class FilterMapping : Profile
    {
            public FilterMapping()
            {
                CreateMap<VehicleQueryResource, VehicleQuery>();
                CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));
            }

    }
}