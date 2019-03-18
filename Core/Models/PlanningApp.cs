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
        public int StateInitialiserId { get; set; }
        public StateInitialiser StateInitialiser { get; set; }
        public int CurrentPlanningStatusId { get; set; }
        public StateStatus CurrentPlanningStatus { get; set; }
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

        //Version 2 multiple generators per project
        // public PlanningApp GeneratePlanningStatesV2(ProjectGenerator projectGenerator, 
        //                                             IEnumerable<StateStatus> stateStatus) 
        // {
        //     var currentDate = SystemDate.Instance.date;
        //     int generatorOrder = 0;   // keeps order of generators/states
        //     var statusOnTime = stateStatus.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();

        //     foreach(var generator in projectGenerator.Generators)
        //     {
        //         foreach(var stateInialiserState in generator.States) 
        //         {
        //             InsertGenNewPlanningStateV2(stateInialiserState, statusOnTime, generatorOrder);
        //         }              
        //         generatorOrder++;
        //     }           

        //     //set first state to current state
        //     if(PlanningAppStates.Count > 0)
        //          PlanningAppStates[0].CurrentState = true;

        //     //Set overall Status to InProgress
        //     CurrentPlanningStatus = stateStatus.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();
        
        //     return this;
        // }

        //Version 2 multiple generators per project
        // public void InsertGenNewPlanningStateV2(StateInitialiserState stateInitialiserState, StateStatus stateStatus, int generatorOrder) 
        // {
        //     PlanningAppState newPlanningAppState = new PlanningAppState();
        //     PlanningAppState prevState;
        //     var stateCount = PlanningAppStates.Count;
        //     if(stateCount > 0) {
        //         prevState =  PlanningAppStates[stateCount-1];
        //         newPlanningAppState.DueByDate =  prevState.DueByDate.AddBusinessDays(stateInitialiserState.CompletionTime);
        //     }
        //     else 
        //         newPlanningAppState.DueByDate = SystemDate.Instance.date.AddBusinessDays(stateInitialiserState.CompletionTime);

        //     //Add custom fields to state if exist
        //     foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
        //         newPlanningAppState.customFields
        //                 .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
        //     }

        //     newPlanningAppState.GeneratorOrder = generatorOrder;
        //     newPlanningAppState.state = stateInitialiserState;
        //     newPlanningAppState.StateStatus = stateStatus;

        //     PlanningAppStates.Add(newPlanningAppState);
        // }

        //===========================================================================================
        //Version 1 Single Generator Per Planning App
        // public PlanningApp GeneratePlanningStates(List<StateInitialiserState> stateInitialiserStates, 
        //                                             IEnumerable<StateStatus> stateStatus) 
        // {
        //     var currentDate = SystemDate.Instance.date;

        //     foreach(var stateInialiserState in stateInitialiserStates) {
        //         PlanningAppState newPlanningAppState = new PlanningAppState();
        //         newPlanningAppState.state = stateInialiserState;

        //         PlanningAppState prevState;
        //         var stateCount = PlanningAppStates.Count;
        //         if(stateCount > 0) {
        //             prevState =  PlanningAppStates[stateCount-1];
        //             newPlanningAppState.DueByDate =  prevState.DueByDate.AddBusinessDays(stateInialiserState.CompletionTime);
        //         }
        //         else 
        //             newPlanningAppState.DueByDate = currentDate.AddBusinessDays(stateInialiserState.CompletionTime);

        //         newPlanningAppState.StateStatus = stateStatus.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
        //         //Add custom fields to state if exist
        //         foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
        //             newPlanningAppState.customFields
        //                     .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
        //         }
        //         PlanningAppStates.Add(newPlanningAppState);
        //     }
        //     //set first state to current state
        //     if(PlanningAppStates.Count > 0)
        //          PlanningAppStates[0].CurrentState = true;

        //     //Set overall Status to InProgress
        //     CurrentPlanningStatus = stateStatus.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();
        
        //     return this;
        // }
       public PlanningApp InsertNewPlanningState(StateInitialiserState newStateInitialiserState, IEnumerable<StateStatus> stateStatus) 
        {

            if(!Completed()) {
                var currentState = Current();

                if(newStateInitialiserState.OrderId > currentState.state.OrderId) {

                    //Remove states after current state
                    var states = this.PlanningAppStates.ToList();

                    PlanningAppState newState = new PlanningAppState();
                    newState.state = newStateInitialiserState;
                    newState.CurrentState = false;
                    newState.StateStatus = stateStatus.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
                    newState.CompletionDate = null;
                    newState.DueByDate = DateTime.Now;
                    PlanningAppStates.Add(newState);

                    updateDueByDates();
                }
            }
            return this;
        }



        public void RemovePlanningState(StateInitialiserState stateInitialiserState) 
        {
            if(!Completed()) {
                var currentState = Current();
                if(stateInitialiserState.OrderId > currentState.state.OrderId) {
                        var planningStateToRemove = this.PlanningAppStates.Where(s => s.state.Id == stateInitialiserState.Id).SingleOrDefault();
                        if(planningStateToRemove != null) {
                            this.PlanningAppStates.Remove(planningStateToRemove);
                            updateDueByDates();
                        }
                }
            }  
        }   

        public void generateDueByDates()
        {
            if(!Completed()) {

                //Important - put states in order before processing!!
                PlanningAppStates = PlanningAppStates.OrderBy(s => s.state.OrderId).ToList();
                var prevState = new PlanningAppState();
                var currState = Current();
                var resetCurrent = Current();

                while(currState != null) {
                    if(!isFirstState(currState)) {
                        prevState = SeekPrev();
                        currState.AggregateDueByDate(prevState);
                    }
                    else 
                        currState.SetDueByDate();
                    
                    currState = Next(currState);
                }               
                //Set original state 
                SetCurrent(resetCurrent);
            }
        }
  
        public void updateDueByDates()  //Called when inserting a new state to an existing planning app
        {
            if(!Completed()) {

                //Important - put states in order before processing!!
                PlanningAppStates = PlanningAppStates.OrderBy(s => s.state.OrderId).ToList();
                var prevState = new PlanningAppState();
                var currState = Current();
                var resetCurrent = Current();

                while(currState != null) {
                    if(!isFirstState(currState)) {
                        prevState = SeekPrev();
                        currState.AggregateDueByDate(prevState);
                    }               
                    currState = Next(currState);
                }               
                //Set original state 
                SetCurrent(resetCurrent);
            }
        }

        public void NextState(List<StateStatus> statusList)
        {

            if(!Completed()) {
                PlanningAppStates = PlanningAppStates.OrderBy(s => s.state.OrderId).ToList();

                var currentDate = SystemDate.Instance.date;
                var prevState = Current(); //Store reference to current state

                if(!isLastState(prevState)) {
                        SeekNext().CurrentState = true;   //move to next state
                }  
                prevState.CompleteState(currentDate, statusList ); //Close out previouse state
                //If Overran then roll all future completion dates by business days overdue
                if(currentDate > prevState.DueByDate) {   
                    var daysDiff = prevState.DueByDate.GetBusinessDays(currentDate, new List<DateTime>());           
                    RollForwardDueByDates(daysDiff, prevState);  
                }  
               
            }
            if(Completed()) {
                CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.Complete).SingleOrDefault();
            }
        }

        public void PrevState(List<StateStatus> statusList)
        {   
            var current = Current();
            if(current != null)   //Cannot Role back a completed application
            {
                if(!isFirstState(current))
                {
                    var prevState = SeekPrev();
                    //daysDiff is used to subtract the future DueByStates by days overrun, basically resetting
                    var daysDiff = prevState.DueByDate.GetBusinessDays(prevState.CompletionDate.Value, new List<DateTime>());
                    rewindState();

                    if(daysDiff > 0)  {
                        var statesToUpdate = RollbackDueByDates(daysDiff, current);   
                    }   
                }
            }
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
     
        private void rewindState()
        {
            var current = Current(); //Make a copy
 
            //Make previous state active
            SeekPrev().CompletionDate = null;
            SeekPrev().CurrentState = true;
  
            //set current non active
            current.CurrentState = false;
        }   
        private void RollForwardDueByDates(int daysDiff, PlanningAppState prevState)
        {
            if(!Completed()) {
                PlanningAppStates
                        .Where(s => s.DueByDate > prevState.DueByDate)
                        .Select(c => {c.DueByDate = c.DueByDate.AddBusinessDays(daysDiff); return c;})
                        .ToList();  
            }
        }
        private List<PlanningAppState> RollbackDueByDates(int daysDiff, PlanningAppState current)
        {
            return PlanningAppStates
                    .Where(s => s.DueByDate > current.DueByDate)
                    .Select(c => {c.DueByDate = c.DueByDate.AddBusinessDays(-daysDiff); return c;})
                    .ToList();  
        }



        //****************/
        //Helper functions

        //Planning id for customer usage
        public void genCustomerReferenceId(Customer customer) {

            //Get List of Designers and Surveyors and tag to reference number
            //TODO NOTE: Refactor!!!!!!
            var drawersInitialsList = Drawers.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var drawersInitials = string.Join(StringConstants.IDil, drawersInitialsList).ToString().TrimEnd(StringConstants.IDil);
  
            var surveyorsInitialsList = Surveyors.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var surveyorsInitials = string.Join(StringConstants.IDil, surveyorsInitialsList).ToString().TrimEnd(StringConstants.IDil);

            var adminsInitialsList = Admins.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var adminsInitials = string.Join(StringConstants.IDil, adminsInitialsList).ToString().TrimEnd(StringConstants.IDil);
          
            //TODO CDS -> take from settings file or database
            PlanningReferenceId = "CDS/" + this.Id.ToString("D6") + "/"                                      
                                        + adminsInitials + '/'
                                        + surveyorsInitials + '/'
                                        + drawersInitials ;

        }



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

        public DateTime MinDueByDate() {
            if(SeekPrev() != null)
                return SeekPrev().DueByDate;
            else   
                return SystemDate.Instance.date;

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
        public bool isLastGeneratorState(int stateInitialiserStateId)
        {
            var r = OrderedPlanningAppStates.GetEnumerator();
            while(r.MoveNext()) {
                PlanningAppState prev = r.Current;
                if(r.Current.StateInitialiserStateId == stateInitialiserStateId) {
                    if(r.MoveNext()) {
                        if(r.Current.GeneratorOrder != prev.GeneratorOrder)
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