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
        public int GeneratorOrder { get; set; }  //States must be kept in order  GeneratorOrder/StateOrder
        public string GeneratorName { get; set; } 
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

        public bool Completed() {
            return this.StateStatus.Name == StatusList.Complete || this.StateStatus.Name == StatusList.Overran;
        }

        /* Helper Methods  */
        public override string ToString() {
            return $"{Id} GenOrder:{GeneratorOrder} StateOrder:{state.OrderId} StateName:{state.Name}  DueBy:{DueByDate}".ToString();
        }
   
        //Object Method Used For Mapping To Resource
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
        public void AggregateDueByDate(PlanningAppState planningAppState) {
            this.DueByDate = planningAppState.DueByDate.AddBusinessDays(this.CompletionTime());
        }
        public void SetDueByDateFrom(DateTime startDate) {
            this.DueByDate = startDate.AddBusinessDays(this.CompletionTime());
        }
        
        public int CompletionTime() {
            if(this.CustomDurationSet)
                return CustomDuration;
            else    
                return state.CompletionTime;    
        }
        public bool isLastGeneratorState() { 
            return this.PlanningApp.isLastGeneratorState(this.Id);   
        }
        public bool canRemoveGenerator() { 
            return this.PlanningApp.canRemoveGenerator(this);   
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