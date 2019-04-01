using System;
using System.Collections.Generic;
using System.Linq;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Core.Utils;
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
            planningAppState.CurrentState = false;
            //return days diff
            return planningAppState.DueByDate.GetBusinessDays(CompletionDate, new List<DateTime>()); 
        }

        public DateTime SetMinDueByDate(PlanningApp planningApp, PlanningAppState planningAppState) {
            
            DateTime minDueByDate = new DateTime();

            if(!planningApp.Completed()) {
                var current = planningApp.Current();
                var currentDate = SystemDate.Instance.date;
                if(planningAppState.state.OrderId >= current.state.OrderId) {
                    if(planningApp.isFirstState(planningAppState))
                        minDueByDate = currentDate.AddBusinessDays(1); //Add one day
                    else if (planningAppState.CurrentState == true)
                        minDueByDate = currentDate.AddBusinessDays(1); 
                    else if (planningApp.SeekPrev(planningAppState).DueByDate <= currentDate)
                        minDueByDate = currentDate.AddBusinessDays(1); 
                    else 
                        minDueByDate = planningApp.SeekPrev(planningAppState).DueByDate.AddBusinessDays(1);
                }
            }

            return minDueByDate;
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

        public void UpdateCustomDueByDate(PlanningAppState planningAppState, DateTime dueByDate)
        {
            int daysDiff;
            if (dueByDate > planningAppState.DueByDate)
                daysDiff = planningAppState.DueByDate.GetBusinessDays(dueByDate, new List<DateTime>());//Move dates forward
            else
                daysDiff = dueByDate.GetBusinessDays(planningAppState.DueByDate, new List<DateTime>()) * -1; //Move dates back

            if (daysDiff != 0)
            {   //Date are different so customise
                if (planningAppState.CustomDurationSet == true)
                {
                    planningAppState.CustomDuration += daysDiff;
                }
                else
                {
                    planningAppState.CustomDurationSet = true;
                    planningAppState.CustomDuration = (planningAppState.state.CompletionTime + daysDiff);
                }
            }
        }

        public PlanningAppStateCustomField getPlanningAppStateCustomField(PlanningAppState planningAppState,int resourceId) { 
            return planningAppState.customFields
                                    .Where(r => r.StateInitialiserStateCustomFieldId == resourceId).SingleOrDefault();
        }

        // public PlanningApp InsertNewPlanningState(StateInitialiserState newStateInitialiserState, IEnumerable<StateStatus> stateStatus) 
        // {

        //     if(!Completed()) {
        //         var currentState = Current();

        //         if(newStateInitialiserState.OrderId > currentState.state.OrderId) {

        //             //Remove states after current state
        //             var states = this.PlanningAppStates.ToList();

        //             PlanningAppState newState = new PlanningAppState();
        //             newState.state = newStateInitialiserState;
        //             newState.CurrentState = false;
        //             newState.StateStatus = stateStatus.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
        //             newState.CompletionDate = null;
        //             newState.DueByDate = DateTime.Now;
        //             PlanningAppStates.Add(newState);

        //             updateDueByDates();
        //         }
        //     }
        //     return this;
        //}
    }
}