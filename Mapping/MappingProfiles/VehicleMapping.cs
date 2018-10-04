using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Mapping.MappingProfiles
{
    public class VehicleMapping : Profile
    {
        public VehicleMapping()
        {
            CreateMap<SaveVehicleResource, Vehicle>();

            CreateMap<SaveVehicleResource, Vehicle>()
                .ForMember(v => v.Id, opt => opt.Ignore())
                .ForMember(v => v.Features, opt => opt.Ignore())
                .AfterMap((vr, v) => {
                    // Remove unselected features
                    var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId)).ToList();
                    foreach (var f in removedFeatures)
                    v.Features.Remove(f);

                    // Add new features
                    var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature { FeatureId = id }).ToList();   
                    foreach (var f in addedFeatures)
                        v.Features.Add(f);
                });
                
            CreateMap<Vehicle, SaveVehicleResource>()
                .ForMember(vr => vr.Features, 
                    opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Features, 
                    opt => opt.MapFrom(v => v.Features.Select(vf => new KeyValuePairResource { Id = vf.Feature.Id, Name =  vf.Feature.Name })))
                .ForMember(vr => vr.Make,
                    opt => opt.MapFrom(v => v.Model.Make));

        }
    }
}