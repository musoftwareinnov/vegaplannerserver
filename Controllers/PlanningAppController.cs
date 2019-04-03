using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Globalization;
using vega.Extensions.DateTime;
using Microsoft.Extensions.Options;
using vega.Core.Models.Settings;
using vega.Core.Utils;
using Microsoft.AspNetCore.Authorization;
using vegaplannerserver.Core.Models;
using vegaplanner.Core.Models.Security;
using Microsoft.AspNetCore.Identity;
using static vegaplanner.Core.Models.Security.Helpers.Constants.Strings;
using System.Security.Claims;
using vegaplannerserver.Core.Models.Generic;
using vega.Services.Interfaces;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/planningapps")]
    public class PlanningAppController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPlanningAppRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStateInitialiserRepository stateInitialiserRepository;
        private readonly IPlanningAppStateRepository planningAppStateRepository;

        private readonly ICustomerRepository customerRepository;
        private readonly IUserRepository userRepository;

        public IStateStatusRepository statusListRepository { get; }
        public IPlanningAppStateService PlanningAppStateService { get; }
        public IPlanningAppService planningAppService { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public DateSettings dateSettings { get; set; }

        public DateTime CurrentDate { get; set; }
        UserManager<AppUser> userManager { get; set; }

        public PlanningAppController(IMapper mapper, 
                                     IPlanningAppService planningAppService,
                                     IPlanningAppRepository repository, 
                                     IPlanningAppStateRepository planningAppStateRepository, 
                                     IUnitOfWork unitOfWork,
                                     IStateStatusRepository statusListRepository,
                                     ICustomerRepository customerRepository,
                                     IUserRepository userRepository,
                                     IStateInitialiserRepository stateInitialiserRepository,
                                     IPlanningAppStateService PlanningAppStateService,
                                     UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.planningAppStateRepository = planningAppStateRepository;
            this.mapper = mapper;
            this.statusListRepository = statusListRepository;
            this.stateInitialiserRepository = stateInitialiserRepository;
            this.PlanningAppStateService = PlanningAppStateService;
            this.userManager = userManager;
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
            this.planningAppService = planningAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlanningApp([FromBody] CreatePlanningAppResource planningResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planningApp = await planningAppService.Create(planningResource);

            return await GetPlanningApp(planningApp.Id);
        }

        [HttpPut("insertgenerator/{id}")]
        public async Task<IActionResult> InsertGenerator(int id, [FromBody] AppGeneratorResource appGenResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planningApp  = await planningAppService.GetPlanningApp(id);
            await planningAppService.InsertGenerator(planningApp, appGenResource.OrderId, appGenResource.GeneratorId);

            await unitOfWork.CompleteAsync();

            return await GetPlanningApp(id);

        }

        [HttpPut("removegenerator/{id}")]
        public async Task<IActionResult> RemoveGenerator(int id, [FromBody] AppGeneratorResource appGenResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planningApp  = await planningAppService.GetPlanningApp(id);
            planningAppService.RemoveGenerator(planningApp, appGenResource.OrderId, appGenResource.GeneratorId);

            await unitOfWork.CompleteAsync();

            return await GetPlanningApp(id);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanningApp(int id)
        {              
            var planningApp = await planningAppService.GetPlanningApp(id);

            if (planningApp == null) {
                Console.WriteLine("Planning App Not Found!!!!!");
                return NotFound();          
            }
            var result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
            result.BusinessDate = CurrentDate.SettingDateFormat();

            return Ok(result);
        }

        [HttpGet]
        public QueryResultResource<PlanningAppSummaryResource> GetPlanningApps(PlanningAppQueryResource filterResource)
        {
            var filter = mapper.Map<PlanningAppQueryResource, PlanningAppQuery>(filterResource);
            
            var queryResult = new QueryResult<PlanningApp>();
            if(filterResource.SearchCriteria == true)
                queryResult = repository.GetPlanningAppsSearchCriteria(filter);  
            else 
                 queryResult = repository.GetPlanningApps(filter);             

            return mapper.Map<QueryResult<PlanningApp>, QueryResultResource<PlanningAppSummaryResource>>(queryResult);
        }

        //TODO: REFACTOR state transition

        [HttpPut("nextstate/{id}")]
        public async Task<IActionResult> NextPlanningAppState(int id)
        {
            var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

            if (planningApp == null)
                return NotFound();

            if(PlanningAppStateService.IsValid(planningApp.Current())) {//Check Mandatory Fields Set
                await planningAppService.NextState(planningApp);
                // //TODO!!!!!!!Inject Logger to say what changed state by which user
            }
            else    
                return BadRequest(new { message = "Mandatory Custom Fields Not Set"});

            await unitOfWork.CompleteAsync();           
            return await GetPlanningApp(planningApp.Id);
        }

        // [HttpPut("prevstate/{id}")]
        // public async Task<IActionResult> PrevPlanningAppState(int id)
        // {
        //     DateTime currentDate = DateTime.Now;
        //     var stateStatusList = await statusListRepository.GetStateStatusList(); //List of possible statuses

        //     var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

        //     if (planningApp == null)
        //         return NotFound();

        //     //TODO!!!!!!!Inject Logger to say what changed state by which user
        //     var currentStateId = planningApp.Current().Id;

        //     //get full entities, including custom state rules
        //     var currentState = await planningAppStateRepository.GetPlanningAppState(currentStateId);
        //     if(currentState.isValid())
        //         planningApp.PrevState(stateStatusList);
        //     else
        //         return BadRequest(new { message = "bad request for next state"});
      
        //     repository.UpdatePlanningApp(planningApp);
        //     await unitOfWork.CompleteAsync();

        //     var result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
        //     result.BusinessDate = CurrentDate.SettingDateFormat();
        //     return Ok(result);
        // }


        // [HttpPut("terminate/{id}")]
        // public async Task<IActionResult> TerminatePlanningApp(int id)
        // {
        //     DateTime currentDate = DateTime.Now;
        //     var stateStatusList = await statusListRepository.GetStateStatusList(); //List of possible statuses

        //     var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

        //     if (planningApp == null)
        //         return NotFound();

        //     //TODO!!!!!!!Inject Logger to say what changed state by which user
        //     var currentStateId = planningApp.Current().Id;

        //     //get full entities, including custom state rules
        //     var currentState = await planningAppStateRepository.GetPlanningAppState(currentStateId);
        //     if(currentState.isValid())
        //         planningApp.Terminate(stateStatusList);
        //     else
        //         return BadRequest(new { message = "bad request for next state"});
      
        //     repository.UpdatePlanningApp(planningApp);
        //     await unitOfWork.CompleteAsync();

        //     var result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
        //     result.BusinessDate = CurrentDate.SettingDateFormat();
        //     return Ok(result);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanningApp(int id, [FromBody] UpdatePlanningAppResource planningResource)
        {
            var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

            if (planningApp == null)
                return NotFound();

            PlanningApp updatedPlanningApp = mapper.Map<UpdatePlanningAppResource, PlanningApp>(planningResource, planningApp);           
                
            repository.UpdatePlanningApp(updatedPlanningApp);
            await unitOfWork.CompleteAsync();

            var result = mapper.Map<PlanningApp, PlanningAppResource>(updatedPlanningApp);
            result.BusinessDate = CurrentDate.SettingDateFormat();

            return Ok(result);
        }



        private void contravar(List<string> planningAppUser) {

        }   
    }
}