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
                                    ICustomerRepository CustomerRepository,
                                    IDateService dateService,
                                    IUnitOfWork unitOfWork)
        {
            this.Mapper = mapper;
            this.PlanningAppRepository = PlanningAppRepository;
            this.ProjectGeneratorRepository = projectGeneratorRepository;
            this.StateStatusRepository = stateStatusRepository;
            this.CustomerRepository = CustomerRepository;
            this.DateService = dateService;
            this.UnitOfWork = unitOfWork;

            statusList = StateStatusRepository.GetStateStatusList().Result;
        }

        public IMapper Mapper { get; }
        public IProjectGeneratorRepository ProjectGeneratorRepository { get; }
        public IStateStatusRepository StateStatusRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IDateService DateService { get; }
        public IUnitOfWork UnitOfWork { get; }
        public IPlanningAppRepository PlanningAppRepository  { get; }
        public List<StateStatus> statusList  { get; }

        public  Task<PlanningApp> Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            planningApp.ProjectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;
            Console.WriteLine("Creating New Planning App, Project Generator -> " + planningApp.ProjectGenerator.Name);

            //Create States
            planningApp = CreatePlanningStates(planningApp);
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
            return PlanningAppRepository.GetPlanningApp(planningApp.Id);
        }
        


        private PlanningApp CreatePlanningStates(PlanningApp planningApp) 
        {
            foreach(var gen in planningApp.ProjectGenerator.OrderedGenerators) {
                Console.WriteLine("Adding Generator " + gen.Generator.Name + " To Planning App");
                foreach(var state in gen.Generator.OrderedStates) {
                    Console.WriteLine("Adding States " + state.Name + " To Planning App");   
                    planningApp = AddPlanningState(planningApp, gen, state);
                }
            }

            //set first state to current state
            if(planningApp.PlanningAppStates.Count > 0)
                 planningApp.PlanningAppStates.FirstOrDefault().CurrentState = true;

            //Set overall Status to InProgress
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();

            return planningApp;
        }

        private PlanningApp AddPlanningState(PlanningApp planningApp, ProjectGeneratorSequence seq, StateInitialiserState stateInitialiserState) 
        {
            //var currentDate = SystemDate.Instance.date;
            var currentDate = DateService.GetCurrentDate();
            PlanningAppState newPlanningAppState = new PlanningAppState();
            newPlanningAppState.state = stateInitialiserState;

            PlanningAppState prevState;
            var stateCount = planningApp.PlanningAppStates.Count;
            if(stateCount > 0) {
                prevState =  planningApp.PlanningAppStates[stateCount-1];
                newPlanningAppState.DueByDate =  prevState.DueByDate.AddBusinessDays(stateInitialiserState.CompletionTime);
            }
            else 
                newPlanningAppState.DueByDate = currentDate.AddBusinessDays(stateInitialiserState.CompletionTime);

            //Add custom fields to state if exist
            foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
                newPlanningAppState.customFields
                        .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            }
 
            newPlanningAppState.GeneratorOrder = seq.SeqId;
            newPlanningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();;

            //Add the State To The Planning Application
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
    }
}