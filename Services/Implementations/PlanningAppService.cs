using System;
using System.Linq;
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
                                    IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            PlanningAppRepository = planningAppRepository;
            ProjectGeneratorRepository = projectGeneratorRepository;
            StateStatusRepository = stateStatusRepository;
            UnitOfWork = unitOfWork;
        }

        public IMapper Mapper { get; }
        public IProjectGeneratorRepository ProjectGeneratorRepository { get; }
        public IStateStatusRepository StateStatusRepository { get; }
        public IUnitOfWork UnitOfWork { get; }

        private readonly IPlanningAppRepository planningAppRepository;
        public  PlanningApp Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            Console.WriteLine("Creating New Planning App, Project Generator -> " + planningApp.ProjectGenerator.Name);
            planningApp.ProjectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;

            //Create States
            planningApp = CreatePlanningStates(planningApp);
            Console.WriteLine("Generated " + planningApp.PlanningAppStates.Count + " Planning States");

            //Create Customer
            

            //Create Fees
            // foreach(var fee in vegaDbContext.Fees) {
            //     PlanningAppFees planningAppFees = new PlanningAppFees { Amount = fee.DefaultAmount, Fee = fee};
            //     planningApp.Fees.Add(planningAppFees);
            // }
            //SavePlanningApp(planningApp);

            UnitOfWork.CompleteAsync();

            //Retrieve planning app from the database and return results
            //var pa = GetPlanningApp(planningApp.Id);

            return planningApp;
        }

        public PlanningApp GetPlanningApp(int id)
        {
            var pa = planningAppRepository.GetPlanningApp(id).Result;
            return pa;         
        }
        
        public void SavePlanningApp(PlanningApp planningApp)
        {
            planningAppRepository.Add(planningApp);                       
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
            return planningApp;
        }

        private PlanningApp AddPlanningState(PlanningApp planningApp, ProjectGeneratorSequence seq, StateInitialiserState stateInitialiserState) 
        {
            var currentDate = SystemDate.Instance.date;
            var statusList = StateStatusRepository.GetStateStatusList().Result;
            
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

            newPlanningAppState.StateStatus = statusList.Where(s => s.Name == StatusList.OnTime).SingleOrDefault();
            //Add custom fields to state if exist
            foreach(var stateInitialiserStateCustomField in newPlanningAppState.state.StateInitialiserStateCustomFields) {
                newPlanningAppState.customFields
                        .Add(new PlanningAppStateCustomField { StateInitialiserStateCustomFieldId = stateInitialiserStateCustomField.StateInitialiserCustomFieldId });
            }
            planningApp.PlanningAppStates.Add(newPlanningAppState);
            
            //set first state to current state
            if(planningApp.PlanningAppStates.Count > 0)
                 planningApp.PlanningAppStates[0].CurrentState = true;

            //Set overall Status to InProgress
            planningApp.CurrentPlanningStatus = statusList.Where(s => s.Name == StatusList.AppInProgress).SingleOrDefault();
        
            return planningApp;
        }
    }
}