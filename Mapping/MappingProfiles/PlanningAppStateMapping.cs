using AutoMapper;
using Microsoft.Extensions.Options;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core.Models.Settings;
using System;
using System.Globalization;
using vega.Extensions.DateTime;
using System.Linq;
using vega.Services.Interfaces;

namespace vega.Mapping.MappingProfiles
{
    public class PlanningAppStateMapping : Profile
    {
        //private readonly DateFormatSetting options;
        public PlanningAppStateMapping()
        {
            CreateMap<PlanningAppState, PlanningAppStateResource>()
                .ForMember(psr => psr.GeneratorId,
                    opt => opt.MapFrom(ps => ps.state.StateInitialiserId))
                .ForMember(psr => psr.StateName,
                    opt => opt.MapFrom(ps => ps.state.Name))
                .ForMember(psr => psr.DueByDate,
                    opt => opt.MapFrom(ps => ps.DueByDate.SettingDateFormat())) //TODO Get from settings
                .ForMember(psr => psr.DateCompleted,
                    opt => opt.MapFrom(ps => ps.CompletionDate == null ? "" : ps.CompletionDate.Value.ToLocalTime().SettingDateFormat()))
                .ForMember(psr => psr.StateStatus,
                    opt => opt.MapFrom(ps => ps.DynamicStateStatus()))
                .ForMember(sis => sis.mandatoryFieldsSet,
                    opt => opt.MapFrom(s => s.mandatoryFieldsSet()))
                .ForMember(psr => psr.isLastGeneratorState,
                    opt => opt.MapFrom(ps => ps.isLastGeneratorState()));

            CreateMap<PlanningAppState, PlanningAppStateFullResource>()
                .ForMember(psr => psr.StateName,
                    opt => opt.MapFrom(ps => ps.state.Name))
                .ForMember(psr => psr.DueByDate,
                    opt => opt.MapFrom(ps => ps.DueByDate.SettingDateFormat())) 
                .ForMember(psr => psr.DateCompleted,
                    opt => opt.MapFrom(ps => ps.CompletionDate == null ? "" : ps.CompletionDate.Value.ToLocalTime().SettingDateFormat()))
                .ForMember(psr => psr.StateStatus,
                    opt => opt.MapFrom(ps => ps.DynamicStateStatus()))
                .ForMember(psr => psr.isLastGeneratorState,
                    opt => opt.MapFrom(ps => ps.isLastGeneratorState()))
                .ForMember(sis => sis.PlanningAppStateCustomFieldsResource,
                    opt => opt.MapFrom(s => s.state.StateInitialiserStateCustomFields
                        .Select(sr => new PlanningAppStateCustomFieldResource {   Id = sr.StateInitialiserCustomField.Id, 
                                                        Name = sr.StateInitialiserCustomField.Name,
                                                        Value = "",
                                                        Type = sr.StateInitialiserCustomField.Type,
                                                        isPlanningAppField = sr.StateInitialiserCustomField.isPlanningAppField,
                                                        isMandatory = sr.StateInitialiserCustomField.isMandatory})));
   
        }

        //public IDateService DateService { get; }
    }
}