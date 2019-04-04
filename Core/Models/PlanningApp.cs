using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using vega.Controllers.Resources;
using vega.Core.Models.Generic;
using vega.Core.Models.States;
using vega.Core.Utils;
using vega.Extensions.DateTime;
using vega.Persistence;
using vegaplanner.Core.Models;
using vegaplanner.Core.Models.Security;
using vegaplannerserver.Core.Models;

namespace vega.Core.Models
{
    public class PlanningApp : IdNameProperty
    {
        // public int PlanningAppStatusId { get; set; }
        // public PlanningAppStatus PlanningAppStatus { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public String PlanningReferenceId { get; set; }
        public int ProjectGeneratorId { get; set; }
        public ProjectGenerator ProjectGenerator { get; set; }

        // Deprecate Single Generator Use
        // public int StateInitialiserId { get; set; }
        // public StateInitialiser StateInitialiser { get; set; }
        public int CurrentPlanningStatusId { get; set; }
        public StateStatus CurrentPlanningStatus { get; set; }
        public DateTime StartDate  { get; set; }
        public string ApplicationNo { get; set; }
        public Contact Developer { get; set; }
        public Address DevelopmentAddress { get; set; }
        
        // public int DevelopmentTypeId { get; set; }
        // public DevelopmentType DevelopmentType { get; set; }
        public String SearchCriteria { get; set; }

        // public Contact CaseOfficer { get; set; }
        // public string CaseOfficer  { get; set; }
        // 
        public IList<PlanningAppState> PlanningAppStates { get; set; }
        
        [NotMapped]
        public IList<PlanningAppState> OrderedPlanningAppStates {
            get { return this.PlanningAppStates.OrderBy(o => o.GeneratorOrder)
                                                    .ThenBy(o => o.state.OrderId)
                                                        .ToList(); } 
        }
        public ICollection<Drawing> Drawings { get; set; }
        public ICollection<PlanningAppSurveyors> Surveyors { get; set; }
        public ICollection<PlanningAppDrawers> Drawers { get; set; }
        public ICollection<PlanningAppAdmins> Admins { get; set; }
        public ICollection<PlanningAppFees> Fees { get; set; }
        public string DescriptionOfWork { get; set; }
        public string Notes { get; set; }

        public PlanningApp()
        {
            PlanningAppStates = new List<PlanningAppState>();
            Drawings = new Collection<Drawing>();
            Surveyors = new Collection<PlanningAppSurveyors>();
            Drawers = new Collection<PlanningAppDrawers>();
            Admins = new Collection<PlanningAppAdmins>();
            Fees = new Collection<PlanningAppFees>();
        }  

        public void Terminate(List<StateStatus> statusList)
        {   
            CurrentPlanningStatus = statusList.Where(p => p.Name == StatusList.AppTerminated).SingleOrDefault();
            //this.Current().CompletionDate = SystemDate.Instance.date;
            
        }

        public void UpdateKeyFields(IEnumerable<PlanningAppStateCustomFieldResource> fieldsToUpdate)
        {   
            foreach(var rule in fieldsToUpdate) {
                switch (rule.Name) {
                    case "ApplicationNo":
                        this.ApplicationNo = rule.Value;
                    break;
                }
            }
        }

        //Private Functions
    
        public void SetCurrent(PlanningAppState planningAppState) {

            foreach ( var state  in OrderedPlanningAppStates) {   
                state.CurrentState = false;
            }
            OrderedPlanningAppStates[OrderedPlanningAppStates.IndexOf(planningAppState)].CurrentState = true;
        }

        public PlanningAppState Next(PlanningAppState planningAppState)
        {       
            PlanningAppState nextState = new PlanningAppState();
            if(!isLastState(planningAppState)) {
                nextState = OrderedPlanningAppStates[OrderedPlanningAppStates.IndexOf(planningAppState)+1];
                planningAppState.CurrentState = false;
                nextState.CurrentState = true;
                return nextState;
            }
            else    
                return null;
        }
        public PlanningAppState SeekNext()
        {       
            if(!Completed() && !isLastState(Current()))
                return OrderedPlanningAppStates[OrderedPlanningAppStates.IndexOf(Current())+1]; 
            else    
                return null;
        }

        public PlanningAppState SeekPrev()  //Get previous state based on CurrentState
        {   
            if(!Completed() && !isFirstState(Current()))
                return OrderedPlanningAppStates[OrderedPlanningAppStates.IndexOf(Current())-1]; 
            else    
                return null;
        }

        public PlanningAppState SeekPrev(PlanningAppState planningAppState) //Get previous state based on specified State
        {   
            if(!Completed() && !isFirstState(planningAppState))
                return OrderedPlanningAppStates[OrderedPlanningAppStates.IndexOf(planningAppState)-1]; 
            else    
                return null;
        }

        public bool Completed()
        { 
             return OrderedPlanningAppStates.Where(p => p.CurrentState == true).Count() == 0;
        }

        public PlanningAppState Current()
        {
                return OrderedPlanningAppStates.Where(s => s.CurrentState == true).SingleOrDefault();
        }

        public DateTime CompletionDate() {
            if(!Completed())
                return SeekLastState().DueByDate;
            else   
                return SeekLastState().CompletionDate.Value;
        }

        public bool isLastState(PlanningAppState planningAppState)
        {
                return OrderedPlanningAppStates.Count() == (OrderedPlanningAppStates.IndexOf(planningAppState) + 1);
        }

        public bool isFirstState(PlanningAppState planningAppState)
        {
                return OrderedPlanningAppStates.IndexOf(planningAppState) == 0;
        }
        public PlanningAppState FirstState()
        {
                return OrderedPlanningAppStates.FirstOrDefault();
        }
        private PlanningAppState SeekLastState()
        {
                return OrderedPlanningAppStates.Count() > 0 ? OrderedPlanningAppStates[OrderedPlanningAppStates.Count() - 1] : null;
                
        }

        private PlanningAppState FirstState(PlanningAppState planningAppState)
        {
                return OrderedPlanningAppStates.Count() > 0 ? OrderedPlanningAppStates[0] : null;
        }
        public bool isLastGeneratorState(int planningAppStateId)
        {
            var r = OrderedPlanningAppStates.GetEnumerator();
            while(r.MoveNext()) {
                PlanningAppState prev = r.Current;
                if(r.Current.Id == planningAppStateId) {
                    if(r.MoveNext()) {
                        if(r.Current.GeneratorOrder > prev.GeneratorOrder && 
                            !prev.Completed()) //Cannot add generator if completed last gen state
                            return true;
                        else
                            return false;
                    }
                    else 
                        return true;
                }
            }
            return false;
        }
    }
}