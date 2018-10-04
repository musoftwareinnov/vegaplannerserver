using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using vega.Core.Models.Generic;
using vega.Core.Models.States;
using vega.Core.Utils;
using vega.Extensions.DateTime;

namespace vega.Core.Models
{
    public class PlanningAppState
    {

        public int Id { get; set; }

        public int PlanningAppId { get; set; }
        public PlanningApp PlanningApp { get; set; }
        public int StateInitialiserStateId { get; set; }
        public StateInitialiserState state { get; set; }
        public DateTime DueByDate { get; set; }

        public bool userModifiedDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public int StateStatusId { get; set; }
        public StateStatus StateStatus { get; set; }
   
        public bool CurrentState { get; set; }
        public StateStatusSettings Options { get; }
        public bool CustomDurationSet { get; set; }
        public int CustomDuration { get; set; }
        public List<PlanningAppStateCustomField> customFields  { get; set; }          
        public string Notes { get; set; }
   
        public PlanningAppState()
        {
            customFields = new List<PlanningAppStateCustomField>();
        }

        public PlanningAppStateCustomField getPlanningAppStateCustomField(int resourceId) { 
            return this.customFields
                                    .Where(r => r.StateInitialiserStateCustomFieldId == resourceId).SingleOrDefault();
        }

        /* Helper Methods  */

        public bool isValid() {
            
            foreach(var template in this.state.StateInitialiserStateCustomFields) {
                var value = getPlanningAppStateCustomField(template.StateInitialiserCustomFieldId);

                if(string.IsNullOrWhiteSpace(value.StrValue) && template.StateInitialiserCustomField.isMandatory)
                    return false;
            }
            return true;
        }

        public int CompleteState(DateTime completionDate, List<StateStatus> stateStatusList) 
        {
            CurrentState = false;
            CompletionDate = completionDate;

            if(CompletionDate > DueByDate)
                StateStatus = stateStatusList.Where(s => s.Name == StatusList.Overran).SingleOrDefault();
            else 
                StateStatus = stateStatusList.Where(s => s.Name == StatusList.Complete).SingleOrDefault();

            return DateTime.Compare(CompletionDate.Value, DueByDate); 
        }
   
        public string DynamicStateStatus() {
            var alertDate = DueByDate.AddBusinessDays(state.AlertToCompletionTime * -1);
        
            var CurrentDate = SystemDate.Instance.date;
            if(CompletionDate == null  )
            {
                if(CurrentDate > DueByDate)
                    return StatusList.Overdue;
                else if (CurrentDate >= alertDate && CurrentDate <= DueByDate)
                    return StatusList.Due;          
                else   
                    return StatusList.OnTime;
            }
            return StateStatus.Name;
        }

        public DateTime SetMinDueByDate(PlanningApp planningApp) {
            
            DateTime minDueByDate = new DateTime();

            if(!planningApp.Completed()) {
                var current = planningApp.Current();
                var currentDate = SystemDate.Instance.date;
                if(this.state.OrderId >= current.state.OrderId) {
                    if(planningApp.isFirstState(this))
                        minDueByDate = currentDate.AddBusinessDays(1); //Add one day
                    else if (this.CurrentState == true)
                        minDueByDate = currentDate.AddBusinessDays(1); 
                    else if (planningApp.SeekPrev(this).DueByDate <= currentDate)
                        minDueByDate = currentDate.AddBusinessDays(1); 
                    else 
                        minDueByDate = planningApp.SeekPrev(this).DueByDate.AddBusinessDays(1);
                }
            }

            return minDueByDate;
        }

        public void AggregateDueByDate(PlanningAppState planningAppState) {
            this.DueByDate = planningAppState.DueByDate.AddBusinessDays(this.CompletionTime());
        }
        public void SetDueByDate() {
            this.DueByDate = SystemDate.Instance.date.AddBusinessDays(this.CompletionTime());
        }

        public void UpdateCustomDueByDate(DateTime dueByDate)
        {
            int daysDiff;
            if (dueByDate > this.DueByDate)
                daysDiff = this.DueByDate.GetBusinessDays(dueByDate, new List<DateTime>());//Move dates forward
            else
                daysDiff = dueByDate.GetBusinessDays(this.DueByDate, new List<DateTime>()) * -1; //Move dates back

            if (daysDiff != 0)
            {   //Date are different so customise
                if (this.CustomDurationSet == true)
                {
                    this.CustomDuration += daysDiff;
                }
                else
                {
                    this.CustomDurationSet = true;
                    this.CustomDuration = (this.state.CompletionTime + daysDiff);
                }
            }
        }
        
        public int CompletionTime() {
            if(this.CustomDurationSet)
                return CustomDuration;
            else    
                return state.CompletionTime;    
        }

        public bool isCustomDuration() {
            return this.CustomDurationSet;
        }

        public bool mandatoryFieldsSet() {  

            //get number of mandatory fields
            int  mandatoryCount = this.state.StateInitialiserStateCustomFields
                                    .Where(m => m.StateInitialiserCustomField.isMandatory==true).Count();

            foreach(var pcf in this.customFields) {
                var scf = this.state.StateInitialiserStateCustomFields
                                .Where(p => p.StateInitialiserCustomFieldId == pcf.StateInitialiserStateCustomFieldId)
                                .SingleOrDefault();

                if(scf != null)
                    if(!string.IsNullOrEmpty(pcf.StrValue) && scf.StateInitialiserCustomField.isMandatory == true)
                        mandatoryCount--;
            }
            return mandatoryCount==0;
         }
    }
}