using System;
using System.Collections.Generic;
using System.Linq;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Extensions.DateTime;
using vega.Services.Interfaces;

namespace vega.Services
{
    public class PlanningAppStateService : IPlanningAppStateService
    {
        public PlanningAppStateService(IDateService dateService,
                                        IStateStatusRepository stateStatusRepository) 
        {
            DateService = dateService;
            StateStatusRepository = stateStatusRepository;
            this.statusList = StateStatusRepository.GetStateStatusList().Result;
            this.CompletionDate = DateService.GetCurrentDate();
        }

        public IDateService DateService { get; }
        public IStateStatusRepository StateStatusRepository { get; }
        private List<StateStatus> statusList  { get; }

        private DateTime CompletionDate { get; }

        public int CompleteState(PlanningAppState planningAppState) {
            if(CompletionDate > planningAppState.DueByDate)
                planningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.Overran).SingleOrDefault();
            else 
                planningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.Complete).SingleOrDefault();

            planningAppState.CompletionDate = CompletionDate;

            //return days diff
            return planningAppState.DueByDate.GetBusinessDays(CompletionDate, new List<DateTime>()); 
        }

        public bool IsValid(PlanningAppState planningAppState) {
            
            foreach(var template in planningAppState.state.StateInitialiserStateCustomFields) {
                 var value = planningAppState.customFields
                                    .Where(r => r.StateInitialiserStateCustomFieldId == template.StateInitialiserCustomFieldId).SingleOrDefault();

                if(string.IsNullOrWhiteSpace(value.StrValue) && template.StateInitialiserCustomField.isMandatory)
                    return false;
            }
            return true;
        }

        public PlanningAppStateCustomField getPlanningAppStateCustomField(PlanningAppState planningAppState,int resourceId) { 
            return planningAppState.customFields
                                    .Where(r => r.StateInitialiserStateCustomFieldId == resourceId).SingleOrDefault();
        }
    }
}