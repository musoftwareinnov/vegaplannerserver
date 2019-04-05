using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Core.Utils;
using vega.Extensions.DateTime;
using vega.Services.Interfaces;
using vegaplanner.Core.Models.Security;
using vegaplannerserver.Core.Models;

namespace vega.Services
{
    public class PlanningAppService : IPlanningAppService
    {

        public PlanningAppService(  IMapper mapper,
                                    IPlanningAppRepository PlanningAppRepository,
                                    IPlanningAppStateRepository PlanningAppStateRepository,
                                    IProjectGeneratorRepository projectGeneratorRepository,
                                    IStateStatusRepository stateStatusRepository,
                                    IStateInitialiserRepository stateInitialiserRepository,
                                    ICustomerRepository CustomerRepository,
                                    IPlanningAppStateService planningAppStateService,
                                    IDateService dateService,
                                    IFeeRepository feeRepository,
                                    UserManager<AppUser> userManager,
                                    IUnitOfWork unitOfWork)
        {
            this.Mapper = mapper;
            this.PlanningAppRepository = PlanningAppRepository;
            this.PlanningAppStateRepository = PlanningAppStateRepository;
            this.ProjectGeneratorRepository = projectGeneratorRepository;
            this.StateStatusRepository = stateStatusRepository;
            this.StateInitialiserRepository = stateInitialiserRepository;
            this.CustomerRepository = CustomerRepository;
            this.PlanningAppStateService = planningAppStateService;
            this.DateService = dateService;
            this.FeeRepository = feeRepository;
            this.UserManager = userManager;
            this.UnitOfWork = unitOfWork;
            this.statusList = StateStatusRepository.GetStateStatusList().Result;
        }

        public IMapper Mapper { get; }
        public IProjectGeneratorRepository ProjectGeneratorRepository { get; }
        public IStateStatusRepository StateStatusRepository { get; }
        public IStateInitialiserRepository StateInitialiserRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IPlanningAppStateService PlanningAppStateService { get; }
        public IDateService DateService { get; }
        public IFeeRepository FeeRepository { get; }
        public UserManager<AppUser> UserManager { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IPlanningAppRepository PlanningAppRepository  { get; }
        public IPlanningAppStateRepository PlanningAppStateRepository  { get; }
        public List<StateStatus> statusList  { get; }

        public  async Task<PlanningApp> Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            //Use when gui setup to create multiple generators
            //planningApp.ProjectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;
            //Console.WriteLine("Creating New Planning App, Project Generator -> " + planningApp.ProjectGenerator.Name);

            //Dummy project multi generator for now
            planningApp.ProjectGeneratorId = planningResource.ProjectGeneratorId;

            planningApp.StartDate = SystemDate.Instance.date;
            //Create Customer
            planningApp.Customer = CustomerRepository.GetCustomer(planningResource.CustomerId).Result;

            //Set Status
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();

            //Create States
            planningApp = await AddSingleGeneratorStates(planningApp);
            Console.WriteLine("Generated " + planningApp.PlanningAppStates.Count + " Planning States");

            //Refactor!!!!!!!
            if(planningResource.Surveyors !=null) {
                foreach(string surveyorId in planningResource.Surveyors) {
                    PlanningAppSurveyors planningAppSurveyors = new PlanningAppSurveyors();
                    planningAppSurveyors.PlanningApp = planningApp;                
                    planningAppSurveyors.AppUser = await UserManager.FindByIdAsync(surveyorId);
                    planningApp.Surveyors.Add(planningAppSurveyors);
                }
            }
            if(planningResource.Drawers !=null)
                foreach(string surveyorId in planningResource.Drawers) {
                    PlanningAppDrawers planningAppDrawers = new PlanningAppDrawers();
                    planningAppDrawers.PlanningApp = planningApp;                
                    planningAppDrawers.AppUser = await UserManager.FindByIdAsync(surveyorId);
                    planningApp.Drawers.Add(planningAppDrawers);
                }
            if(planningResource.Admins !=null)
                foreach(string adminId in planningResource.Admins) {
                    PlanningAppAdmins planningAppAdmins = new PlanningAppAdmins();
                    planningAppAdmins.PlanningApp = planningApp;                
                    planningAppAdmins.AppUser = await UserManager.FindByIdAsync(adminId);
                    planningApp.Admins.Add(planningAppAdmins);
                } 
            //Create Fees
            foreach(var fee in FeeRepository.GetFeesDefault()) {
                PlanningAppFees planningAppFees = new PlanningAppFees { Amount = fee.DefaultAmount, Fee = fee};
                planningApp.Fees.Add(planningAppFees);
            }

            //Persist planning app
            PlanningAppRepository.Add(planningApp); 
            await UnitOfWork.CompleteAsync();

            //Create Reference using planning id
            planningApp.PlanningReferenceId = genCustomerReferenceId(planningApp);
            PlanningAppRepository.UpdatePlanningApp(planningApp); 
            await UnitOfWork.CompleteAsync();

            //Retrieve planning app from database and return results to controller
            return await PlanningAppRepository.GetPlanningApp(planningApp.Id, includeRelated:false);
        }
        
        //Single Generator Creator
        private async Task<PlanningApp> AddSingleGeneratorStates(PlanningApp planningApp) 
        {

            await InsertGenerator(planningApp, 1, planningApp.ProjectGeneratorId) ;

            //set first state to current state
            if(planningApp.PlanningAppStates.Count > 0) {
                var startState = planningApp.PlanningAppStates.FirstOrDefault();
                var currentDate = DateService.GetCurrentDate();
                startState.CurrentState = true;

                InitialiseDuebyDates(planningApp);
            }
            //Set overall Status to InProgress
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();

            return planningApp;
        }

        //NOT USED !!! Multi Generator creator (next version when dine front end)
        private async Task<PlanningApp> AddProjectGeneratorStates(PlanningApp planningApp) 
        {
            // foreach(var gen in planningApp.ProjectGenerator.OrderedGenerators) {
            //     //Console.WriteLine("Adding Generator " + gen.Generator.Name + " To Planning App");
                 //await InsertGenerator(planningApp, gen.SeqId, gen.Generator.Id) ;
                 await InsertGenerator(planningApp,-1,-1) ;
            // }

            //set first state to current state
            if(planningApp.PlanningAppStates.Count > 0) {
                var startState = planningApp.PlanningAppStates.FirstOrDefault();
                var currentDate = DateService.GetCurrentDate();
                startState.CurrentState = true;

                InitialiseDuebyDates(planningApp);
            }
            //Set overall Status to InProgress
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();

            return planningApp;
        }

        public async Task<PlanningApp> InsertGenerator(PlanningApp planningApp, int SequenceId, int NewGeneratorId) 
        {
            var currentDate = DateService.GetCurrentDate();
            PlanningAppState newPlanningAppState = new PlanningAppState();

            var generator = await StateInitialiserRepository.GetStateInitialiser(NewGeneratorId);

            var generatorExists = planningApp.OrderedPlanningAppStates.Any(ps => ps.GeneratorOrder == SequenceId);
            //increase SequenceId of all future generators ad insert new one
            if(generatorExists) {
                planningApp.PlanningAppStates
                .Where(g => g.GeneratorOrder >= SequenceId)
                .Select(g => {g.GeneratorOrder++ ; return g;})
                .ToList();               
            }
    
            foreach(var state in generator.OrderedStates) {
                 InsertPlanningState(planningApp, SequenceId, generator, state );
            }
            InitialiseDuebyDates(planningApp);
            return planningApp;
        }

        public async Task<PlanningApp> AppendGenerator(PlanningApp planningApp, int SequenceId, int NewGeneratorId) 
        {
            var currentDate = DateService.GetCurrentDate();
            PlanningAppState newPlanningAppState = new PlanningAppState();

            var generator = await StateInitialiserRepository.GetStateInitialiser(NewGeneratorId);

            var generatorExists = planningApp.OrderedPlanningAppStates.Any(ps => ps.GeneratorOrder == SequenceId);
            //increase SequenceId of all future generators ad insert new one
            if(generatorExists) {
                planningApp.PlanningAppStates
                .Where(g => g.GeneratorOrder >= SequenceId)
                .Select(g => {g.GeneratorOrder++ ; return g;})
                .ToList();               
            }
    
            foreach(var state in generator.OrderedStates) {
                 InsertPlanningState(planningApp, SequenceId, generator, state );
            }
            UpdateDueByDates(planningApp);
            return planningApp;
        }

        public PlanningApp RemoveGenerator(PlanningApp planningApp, int SequenceId, int GeneratorId) 
        {

            var currentDate = DateService.GetCurrentDate();
            PlanningAppState newPlanningAppState = new PlanningAppState();

            //var generator = await StateInitialiserRepository.GetStateInitialiser(GeneratorId);
            var statesToDelete = planningApp.PlanningAppStates.Where(s => s.GeneratorOrder == SequenceId).ToList();

            foreach(var planningAppState in statesToDelete) {
                planningApp.PlanningAppStates.Remove(planningAppState);
            }
            //Shift states up
            planningApp.OrderedPlanningAppStates
            .Where(g => g.GeneratorOrder >= SequenceId)
            .Select(g => {g.GeneratorOrder-- ; return g;})
            .ToList();               
            
            UpdateDueByDates(planningApp);
            return planningApp;
        }
        private int InitialiseDuebyDates(PlanningApp planningApp)
        {
            int noOfDatesUpdated = 0;
            var startState = planningApp.PlanningAppStates.FirstOrDefault();
            startState.CurrentState = true;   
            startState.DueByDate = DateService.GetCurrentDate().AddBusinessDays(startState.state.CompletionTime);

            if(planningApp.PlanningAppStates.Count>1) 
                noOfDatesUpdated = UpdateDueByDates(planningApp);
            
            return noOfDatesUpdated;
        }

        private string genCustomerReferenceId(PlanningApp planningApp) {

            //Get List of Designers and Surveyors and tag to reference number
            //TODO NOTE: Refactor!!!!!!
            var drawersInitialsList = planningApp.Drawers.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var drawersInitials = string.Join(StringConstants.IDil, drawersInitialsList).ToString().TrimEnd(StringConstants.IDil);
  
            var surveyorsInitialsList = planningApp.Surveyors.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var surveyorsInitials = string.Join(StringConstants.IDil, surveyorsInitialsList).ToString().TrimEnd(StringConstants.IDil);

            var adminsInitialsList = planningApp.Admins.Select(d => d.AppUser.FirstName.Substring(0,1) + 
                                                    d.AppUser.LastName.Substring(0,1)).ToList();

            var adminsInitials = string.Join(StringConstants.IDil, adminsInitialsList).ToString().TrimEnd(StringConstants.IDil);
          
            //TODO CDS -> take from settings file or database
            string reference = "CDS/"   + surveyorsInitials + '/'
                                        + drawersInitials + '/'
                                        + adminsInitials + '/' 
                                        + planningApp.Id.ToString("D6");

            return reference;

        }

        public PlanningApp InsertPlanningState(PlanningApp planningApp, int GeneratorOrder, StateInitialiser generator, StateInitialiserState stateInitialiserState) 
        {
            if(!CanInsertState(planningApp, GeneratorOrder,stateInitialiserState))
                return planningApp;

            PlanningAppState newPlanningAppState = new PlanningAppState();
            newPlanningAppState.state = stateInitialiserState;

            foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
                newPlanningAppState.customFields
                        .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            }

            //Console.WriteLine("Adding States " + stateInitialiserState.Name + " To Planning App"); 
            newPlanningAppState.GeneratorOrder = GeneratorOrder;
            newPlanningAppState.GeneratorName = generator.Name;
            newPlanningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
            planningApp.PlanningAppStates.Add(newPlanningAppState);
            return planningApp;
        }

        private bool CanInsertState(PlanningApp planningApp, int GeneratorOrder, StateInitialiserState stateInitialiserState) 
        {
            if(planningApp.CurrentPlanningStatus != statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault()) {
                return false;
            }

            var currentState = planningApp.Current();
            if(currentState == null)
                return true;  //New Application Being Created
            else {
                if(GeneratorOrder == currentState.GeneratorOrder && stateInitialiserState.OrderId > currentState.state.OrderId)
                    return true;
                else if(GeneratorOrder > currentState.GeneratorOrder)
                    return true;
            }

            return false;
        } 
        private bool CanRemoveState(PlanningApp planningApp, int GeneratorOrder, StateInitialiserState stateInitialiserState) 
        {
            if(planningApp.CurrentPlanningStatus != statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault()) {
                return false;
            }
            var currentState = planningApp.Current();
            if(GeneratorOrder > currentState.GeneratorOrder)
                    return true;

            return false;
        } 
        public Task<PlanningApp> GetPlanningApp(int id)
        {
            var planningApp = PlanningAppRepository.GetPlanningApp(id); 
            return planningApp;                  
        }
        public void SavePlanningApp(PlanningApp planningApp)
        {
            PlanningAppRepository.Add(planningApp);                      
        }

        public Task<PlanningApp> NextState(PlanningApp planningApp) 
        {
            if(!planningApp.Completed())
            {
                var prevState = planningApp.Current();
                if(!planningApp.isLastState(prevState)) {
                        planningApp.SeekNext().CurrentState = true;   //move to next state
                }                  

                var daysDiff = PlanningAppStateService.CompleteState(prevState);
                if(planningApp.Completed()) 
                    planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.Complete).SingleOrDefault();
                else {
                    //If Overran then roll all future completion dates by business days overdue
                    if(daysDiff > 0)          
                        RollForwardDueByDates(planningApp, daysDiff);  
                }
            }
            PlanningAppRepository.UpdatePlanningApp(planningApp);
            
            return GetPlanningApp(planningApp.Id);
        }

        private void RollForwardDueByDates(PlanningApp planningApp, int daysDiff)
        {
            
            if(!planningApp.Completed()) {
                var prevState = planningApp.SeekPrev();
                planningApp.OrderedPlanningAppStates
                        .Where(s => s.DueByDate > prevState.DueByDate)
                        .Select(c => {c.DueByDate = c.DueByDate.AddBusinessDays(daysDiff); return c;})
                        .ToList();  
            }
        }

        public void RemovePlanningState(PlanningApp planningApp, StateInitialiserState stateInitialiserState) 
        {
            if(!planningApp.Completed()) {
                var currentState = planningApp.Current();

                //If state is in current generator and is after the current state then remove
                var statesToRemove = planningApp.PlanningAppStates.Where(s => s.state.Id == stateInitialiserState.Id
                                                                            && s.GeneratorOrder == currentState.GeneratorOrder
                                                                            && s.state.OrderId > currentState.state.OrderId).ToList();
                
                //state in future generators
                statesToRemove.AddRange(planningApp.PlanningAppStates.Where(s => s.state.Id == stateInitialiserState.Id
                                                                            && s.GeneratorOrder > currentState.GeneratorOrder
                                                                            ).ToList());

                                                                        
                foreach(var state in statesToRemove) {
                    planningApp.PlanningAppStates.Remove(state);                
                }
                UpdateDueByDates(planningApp);   
            }  
        } 

        public int UpdateDueByDates(PlanningApp planningApp)  //Called when inserting a new state to an existing planning app
        {
            int statesUpdated = 0;
            if(!planningApp.Completed()) {
                var prevState = new PlanningAppState();
                var currState = planningApp.Current();
                var resetCurrent = planningApp.Current();

                while(currState != null) {
                    if(!planningApp.isFirstState(currState)) {
                        prevState = planningApp.SeekPrev();
                        currState.AggregateDueByDate(prevState);
                    }
                    else 
                        currState.SetDueByDateFrom(planningApp.StartDate); //First State In Project

                    statesUpdated++;                           
                    currState = planningApp.Next(currState);
                }               
                //Set original state 
                planningApp.SetCurrent(resetCurrent);
            }
            return statesUpdated;
        }
    }
}


