using AutoMapper;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Services.Interfaces;

namespace vega.Services
{
    public class PlanningAppService : IPlanningAppService
    {

        public PlanningAppService(  IMapper mapper,
                                    IPlanningAppRepository PlanningAppRepository,
                                    IProjectGeneratorRepository projectGeneratorRepository)
        {
            Mapper = mapper;
            PlanningAppRepository = planningAppRepository;
            ProjectGeneratorRepository = projectGeneratorRepository;
        }

        public IMapper Mapper { get; }
        public IProjectGeneratorRepository ProjectGeneratorRepository { get; }

        private readonly IPlanningAppRepository planningAppRepository;
        public  PlanningApp Create(CreatePlanningAppResource planningResource) {
            
            var planningApp = Mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            var projectGenerator = ProjectGeneratorRepository.GetProjectGenerator(planningResource.ProjectGeneratorId).Result;

            // planningApp.ProjectGenerator = projectGenerator; //Assign Project Generator to Planning App

            // foreach(var gen in projectGenerator.Generators) {
            //     //gen.Generator.Name;
            //     //Add Generator States to the new planning application

                
            // }

            return planningApp;
        }
    }
}