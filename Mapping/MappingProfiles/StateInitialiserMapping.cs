using System.Linq;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Controllers.Resources.StateInitialser;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Mapping.MappingProfiles
{
    public class StateInitialiserMapping : Profile
    {
        public StateInitialiserMapping() {
                CreateMap<StateInitialiser, StateInitialiserResource>();
                CreateMap<StateInitialiserSaveResource, StateInitialiser>();
                CreateMap<SaveStateInitialiserStateResource, StateInitialiserState>();

                CreateMap<StateInitialiserStateResource, StateInitialiserState>()
                                .ForMember(s => s.StateInitialiserStateCustomFields, opt => opt.Ignore());

                CreateMap<StateInitialiserState, StateInitialiserStateResource>()
                    .ForMember(sis => sis.StateInitialiserStateCustomFields,
                        opt => opt.MapFrom(s => s.StateInitialiserStateCustomFields
                            .Select(sr => new StateInitialiserCustomField {   Id = sr.StateInitialiserCustomField.Id, 
                                                            Name = sr.StateInitialiserCustomField.Name,
                                                            Type = sr.StateInitialiserCustomField.Type,
                                                            isPlanningAppField = sr.StateInitialiserCustomField.isPlanningAppField,
                                                            isMandatory = sr.StateInitialiserCustomField.isMandatory})));

        }
    }
}