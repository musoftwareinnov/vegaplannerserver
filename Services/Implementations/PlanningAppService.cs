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
                                    IProjectGeneratorRepository projectGeneratorRepository,
                                    IStateStatusRepository stateStatusRepository,
                                    IStateInitialiserRepository stateInitialiserRepository,
                                    ICustomerRepository CustomerRepository,
                                    IDateService dateService,
                                    IUnitOfWork unitOfWork)
        {
            this.Mapper = mapper;
            this.PlanningAppRepository = PlanningAppRepository;
            this.ProjectGeneratorRepository = projectGeneratorRepository;
            this.StateStatusRepository = stateStatusRepository;
            this.StateInitialiserRepository = stateInitialiserRepository;
            this.CustomerRepository = CustomerRepository;
            this.DateService = dateService;
            this.UnitOfWork = unitOfWork;

            statusList = StateStatusRepository.GetStateStatusList().Result;
        }

        public IMapper Mapper { get; }
        public IProjectGeneratorRepository ProjectGeneratorRepository { get; }
        public IStateStatusRepository StateStatusRepository { get; }
        public IStateInitialiserRepository StateInitialiserRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IDateService DateService { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IPlanningAppRepository PlanningAppRepository  { get; }
        public List<StateStatus> statusList  { get; }

        public  async Task<PlanningApp> Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            planningApp.ProjectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;
            Console.WriteLine("Creating New Planning App, Project Generator -> " + planningApp.ProjectGenerator.Name);

            //Create States
            planningApp = await CreatePlanningStates(planningApp);
            Console.WriteLine("Generated " + planningApp.PlanningAppStates.Count + " Planning States");

            //Create Customer
            planningApp.Customer = CustomerRepository.GetCustomer(planningResource.CustomerId).Result;

            //Create Fees
            // foreach(var fee in vegaDbContext.Fees) {
            //     PlanningAppFees planningAppFees = new PlanningAppFees { Amount = fee.DefaultAmount, Fee = fee};
            //     planningApp.Fees.Add(planningAppFees);
            // }

            PlanningAppRepository.Add(planningApp); 

            //Retrieve planning app from database and return results
            return await PlanningAppRepository.GetPlanningApp(planningApp.Id);
        }
        


        private async Task<PlanningApp> CreatePlanningStates(PlanningApp planningApp) 
        {
            foreach(var gen in planningApp.ProjectGenerator.OrderedGenerators) {
                Console.WriteLine("Adding Generator " + gen.Generator.Name + " To Planning App");

                await InsertGenerator(planningApp.Id, gen.SeqId, gen.Id) ;
                // foreach(var state in gen.Generator.OrderedStates) {
                //     Console.WriteLine("Adding States " + state.Name + " To Planning App");   
                //     planningApp = InsertPlanningState(planningApp, gen.SeqId, state);
                // }
            }

            //set first state to current state
            if(planningApp.PlanningAppStates.Count > 0)
                 planningApp.PlanningAppStates.FirstOrDefault().CurrentState = true;

            //Set overall Status to InProgress
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();

            return planningApp;
        }

        private PlanningApp InsertPlanningState(PlanningApp planningApp, int GeneratorOrder, StateInitialiserState stateInitialiserState) 
        {
            PlanningAppState newPlanningAppState = new PlanningAppState();
            newPlanningAppState.state = stateInitialiserState;

            foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
                newPlanningAppState.customFields
                        .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            }

            newPlanningAppState.GeneratorOrder = GeneratorOrder;
            planningApp.PlanningAppStates.Add(newPlanningAppState);

            //Get Ordered States and roll date forward from this state


            return planningApp;
         }

        // private PlanningApp AddPlanningState(PlanningApp planningApp, ProjectGeneratorSequence GeneratorOrder, StateInitialiserState stateInitialiserState) 
        // {
        //     //var currentDate = SystemDate.Instance.date;
        //     var currentDate = DateService.GetCurrentDate();
        //     PlanningAppState newPlanningAppState = new PlanningAppState();
        //     newPlanningAppState.state = stateInitialiserState;

        //     PlanningAppState prevState;
        //     var stateCount = planningApp.PlanningAppStates.Count;
        //     if(stateCount > 0) {
        //         //Get Previous State
        //         //If 
        //         prevState =  planningApp.OrderedPlanningAppStates[stateCount-1];
        //         newPlanningAppState.DueByDate =  prevState.DueByDate.AddBusinessDays(stateInitialiserState.CompletionTime);
        //     }
        //     else 
        //         newPlanningAppState.DueByDate = currentDate.AddBusinessDays(stateInitialiserState.CompletionTime);

        //     //Add custom fields to state if exist
        //     foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
        //         newPlanningAppState.customFields
        //                 .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
        //     }
 
        //     newPlanningAppState.GeneratorOrder = GeneratorOrder.SeqId;
        //     //Add the State To The Planning Application
        //     planningApp.PlanningAppStates.Add(newPlanningAppState);

        //     return planningApp;
        // }

        public async Task<PlanningApp> InsertGenerator(int planningAppId, int SequenceId, int NewGeneratorId) 
        {
            //var currentDate = SystemDate.Instance.date;
            var currentDate = DateService.GetCurrentDate();
            PlanningAppState newPlanningAppState = new PlanningAppState();

            var planningApp = await PlanningAppRepository.GetPlanningApp(planningAppId);
            var generator = await StateInitialiserRepository.GetStateInitialiser(NewGeneratorId);

            var generatorExists = planningApp.OrderedPlanningAppStates.Any(ps => ps.GeneratorOrder == SequenceId);

            //increase SequenceId of all future generators ad insert new one
            if(generatorExists) {
                planningApp.PlanningAppStates
                .Where(g => g.GeneratorOrder > SequenceId)
                .Select(g => {g.GeneratorOrder++ ; return g;})
                .ToList();               
            }
    
            foreach(var state in generator.OrderedStates) {
                 InsertPlanningState(planningApp, SequenceId, state);
            }


            //IS LAST STATE SHOULD BE RENAMED "Can Insert New Gen" !!! (After current status)
            // //Only add new states if the current state precedes (GUI will prevent this but extra check)
            // if(planningApp.Current().GeneratorOrder <= OrderId) {
            //     //update generator orders by 1
            //     if(!planningApp.Completed()) {
            //         planningApp.PlanningAppStates
            //             .Where(g => g.GeneratorOrder > OrderId)
            //             .Select(g => {g.GeneratorOrder++ ; return g;})
            //             .ToList();  
            //     }
            //     //Get Generator States and insert
            //    // var generator = StateInitialiserRepo
            // }

            // newPlanningAppState.state = stateInitialiserState;

            // PlanningAppState prevState;
            // var stateCount = planningApp.PlanningAppStates.Count;
            // if(stateCount > 0) {
            //     prevState =  planningApp.PlanningAppStates[stateCount-1];
            //     newPlanningAppState.DueByDate =  prevState.DueByDate.AddBusinessDays(stateInitialiserState.CompletionTime);
            // }
            // else 
            //     newPlanningAppState.DueByDate = currentDate.AddBusinessDays(stateInitialiserState.CompletionTime);

            // //Add custom fields to state if exist
            // foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
            //     newPlanningAppState.customFields
            //             .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            // }
 
            // newPlanningAppState.GeneratorOrder = seq.SeqId;
            // //newPlanningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();;

            // //Add the State To The Planning Application
            // planningApp.PlanningAppStates.Add(newPlanningAppState);

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
    }
}