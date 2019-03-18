using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Core.Utils;
using vega.Extensions.DateTime;
using vega.Services.Interfaces;
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
        public IUnitOfWork UnitOfWork { get; }
        public IPlanningAppRepository PlanningAppRepository  { get; }
        public IPlanningAppStateRepository PlanningAppStateRepository  { get; }
        public List<StateStatus> statusList  { get; }

        public  async Task<PlanningApp> Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            planningApp.ProjectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;
            Console.WriteLine("Creating New Planning App, Project Generator -> " + planningApp.ProjectGenerator.Name);

            //Create Customer
            planningApp.Customer = CustomerRepository.GetCustomer(planningResource.CustomerId).Result;

            //Persist new PlanningApp
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();
            // PlanningAppRepository.Add(planningApp); 
            // await UnitOfWork.CompleteAsync();

            //var ps  = await PlanningAppRepository.GetPlanningApp(planningApp.Id, includeRelated:false);

            //Create States
            planningApp = await AddGeneratorStates(planningApp);
            Console.WriteLine("Generated " + planningApp.PlanningAppStates.Count + " Planning States");

            //Create Fees
            // foreach(var fee in vegaDbContext.Fees) {
            //     PlanningAppFees planningAppFees = new PlanningAppFees { Amount = fee.DefaultAmount, Fee = fee};
            //     planningApp.Fees.Add(planningAppFees);
            // }

            PlanningAppRepository.Add(planningApp); 
            await UnitOfWork.CompleteAsync();

            //Retrieve planning app from database and return results to controller
            return await PlanningAppRepository.GetPlanningApp(planningApp.Id, includeRelated:false);
        }
        
        private async Task<PlanningApp> AddGeneratorStates(PlanningApp planningApp) 
        {
            foreach(var gen in planningApp.ProjectGenerator.OrderedGenerators) {
                //Console.WriteLine("Adding Generator " + gen.Generator.Name + " To Planning App");
                await InsertGenerator(planningApp, gen.SeqId, gen.Generator.Id) ;
            }

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
                 InsertPlanningState(planningApp, SequenceId, state);
            }
            InitialiseDuebyDates(planningApp);
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
            
            InitialiseDuebyDates(planningApp);
            return planningApp;
        }
        private int InitialiseDuebyDates(PlanningApp planningApp)
        {
            int noOfDatesUpdated = 0;
            var startState = planningApp.PlanningAppStates.FirstOrDefault();
            startState.CurrentState = true;   
            startState.DueByDate = DateService.GetCurrentDate().AddBusinessDays(startState.state.CompletionTime);

            if(planningApp.PlanningAppStates.Count>1) 
                noOfDatesUpdated = UpdateDuebyDates(planningApp);
            
            return noOfDatesUpdated;
        }
        private int UpdateDuebyDates(PlanningApp planningApp)
        {
            int statesUpdated = 0;
            var ops = planningApp.OrderedPlanningAppStates.GetEnumerator();
            if(ops.MoveNext()) { //First State
                var prevCompletionTime = ops.Current.DueByDate;
                while(ops.MoveNext()) { //Second state
                        ops.Current.DueByDate = prevCompletionTime.AddBusinessDays(ops.Current.CompletionTime());
                        prevCompletionTime = ops.Current.DueByDate;
                        statesUpdated++;
                    }
                }
            return statesUpdated;
        }
 
        private PlanningApp InsertPlanningState(PlanningApp planningApp, int GeneratorOrder, StateInitialiserState stateInitialiserState) 
        {
            PlanningAppState newPlanningAppState = new PlanningAppState();
            newPlanningAppState.state = stateInitialiserState;

            foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
                newPlanningAppState.customFields
                        .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            }

            //Console.WriteLine("Adding States " + stateInitialiserState.Name + " To Planning App"); 
            newPlanningAppState.GeneratorOrder = GeneratorOrder;
            newPlanningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
            planningApp.PlanningAppStates.Add(newPlanningAppState);
            return planningApp;
        }


        public Task<PlanningApp> GetPlanningApp(int id)
        {
            return PlanningAppRepository.GetPlanningApp(id);                     
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
                    if(daysDiff > 0) { 
                        //ToDo!!!          
                        //RollForwardDueByDates(daysDiff, prevState);  
                    } 
                }
            }
            PlanningAppRepository.UpdatePlanningApp(planningApp);
            
            return GetPlanningApp(planningApp.Id);
        }

        public void UpdateDueByDates(PlanningApp planningApp)  //Called when inserting a new state to an existing planning app
        {
            if(!planningApp.Completed()) {
                var prevState = new PlanningAppState();
                var currState = planningApp.Current();
                var resetCurrent = planningApp.Current();

                while(currState != null) {
                    if(!planningApp.isFirstState(currState)) {
                        prevState = planningApp.SeekPrev();
                        currState.AggregateDueByDate(prevState);
                    }               
                    currState = planningApp.Next(currState);
                }               
                //Set original state 
                planningApp.SetCurrent(resetCurrent);
            }
        }
    }
}
                        // OLD UPDATE To DUBY DATE
                        // var stateToUpdateIdx = planningApp.PlanningAppStates.IndexOf(ops.Current);
                        // var stateToUpdate = planningApp.PlanningAppStates[stateToUpdateIdx];
                        // stateToUpdate.DueByDate = prevCompletionTime.AddBusinessDays(stateToUpdate.CompletionTime());
                        // prevCompletionTime = stateToUpdate.DueByDate;
                        // statesUpdated++;
                        // var stateToUpdateIdx = planningApp.PlanningAppStates.IndexOf(ops.Current);
                        // var stateToUpdate = planningApp.PlanningAppStates[stateToUpdateIdx];