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
        public RoleManager<IdentityRole> RoleManager { get; }
        public DateSettings dateSettings { get; set; }

        public DateTime CurrentDate { get; set; }
        UserManager<AppUser> userManager { get; set; }

        public PlanningAppController(IMapper mapper, 
                                     IPlanningAppRepository repository, 
                                     IPlanningAppStateRepository planningAppStateRepository, 
                                     IUnitOfWork unitOfWork,
                                     IStateStatusRepository statusListRepository,
                                     ICustomerRepository customerRepository,
                                     IUserRepository userRepository,
                                     IStateInitialiserRepository stateInitialiserRepository,
                                     UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.planningAppStateRepository = planningAppStateRepository;
            this.mapper = mapper;
            this.statusListRepository = statusListRepository;
            this.stateInitialiserRepository = stateInitialiserRepository;
            this.userManager = userManager;
            this.customerRepository = customerRepository;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlanningApp([FromBody] CreatePlanningAppResource planningResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var planningApp = mapper.Map<CreatePlanningAppResource, PlanningApp>(planningResource);

            var stateInitialiser = await stateInitialiserRepository.GetStateInitialiser(planningApp.StateInitialiserId, includeDeleted: false);
            
            PlanningAppResource result = null;

            if(stateInitialiser.States.Count > 0)
            {
                //REFACTOR SORT NULL PROBLEM, SHOULD BE ALLOCATED IN RESOURCE!!!!!
                if(planningResource.Surveyors !=null)
                foreach(string surveyorId in planningResource.Surveyors) {
                    PlanningAppSurveyors planningAppSurveyors = new PlanningAppSurveyors();
                    planningAppSurveyors.PlanningApp = planningApp;                
                    planningAppSurveyors.AppUser = await userManager.FindByIdAsync(surveyorId);
                    planningApp.Surveyors.Add(planningAppSurveyors);
                }

                if(planningResource.Drawers !=null)
                foreach(string surveyorId in planningResource.Drawers) {
                    PlanningAppDrawers planningAppDrawers = new PlanningAppDrawers();
                    planningAppDrawers.PlanningApp = planningApp;                
                    planningAppDrawers.AppUser = await userManager.FindByIdAsync(surveyorId);
                    planningApp.Drawers.Add(planningAppDrawers);
                }
                if(planningResource.Admins !=null)
                foreach(string adminId in planningResource.Admins) {
                    PlanningAppAdmins planningAppAdmins = new PlanningAppAdmins();
                    planningAppAdmins.PlanningApp = planningApp;                
                    planningAppAdmins.AppUser = await userManager.FindByIdAsync(adminId);
                    planningApp.Admins.Add(planningAppAdmins);
                }

                repository.Add(planningApp, stateInitialiser);
                await unitOfWork.CompleteAsync();

                planningApp = await repository.GetPlanningApp(planningApp.Id, includeRelated: true);

                //Generate Customer Reference
                var customer = await customerRepository.GetCustomer(planningApp.CustomerId,includeRelated:false);
                planningApp.genCustomerReferenceId(customer);
                await unitOfWork.CompleteAsync();  //Save reference number
                
                result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
                result.BusinessDate = CurrentDate.SettingDateFormat();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlanningApp(int id)
        {              
            var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

            if (planningApp == null)
                return NotFound();          

            var result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
            result.BusinessDate = CurrentDate.SettingDateFormat();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlanningAppState(int id, [FromBody] UpdatePlanningAppResource planningResource)
        {
            DateTime currentDate = DateTime.Now;
            var stateStatusList = await statusListRepository.GetStateStatusList(); //List of possible statuses

            var planningApp = await repository.GetPlanningApp(id, includeRelated: true);

            if (planningApp == null)
                return NotFound();

            //TODO!!!!!!!Inject Logger to say what changed state by which user
            if(planningResource.method == (int) StateAction.NextState) {
                
                //Validate that custom mandatory fields have been set
                var currentStateId = planningApp.Current().Id;

                //get full entities, including custom state rules
                var currentState = await planningAppStateRepository.GetPlanningAppState(currentStateId);
                if(currentState.isValid())
                    planningApp.NextState(stateStatusList);
                else
                    return BadRequest(new { message = "bad request for next state"});
            }
            else if (planningResource.method == (int) StateAction.PrevState) 
                planningApp.PrevState(stateStatusList);

            else if (planningResource.method == (int) StateAction.Terminate) 
                planningApp.Terminate(stateStatusList);
            else 
                {
                //No state specified just save details that can be modified by the user
                //PlanningApp res = mapper.Map<UpdatePlanningAppResource, PlanningApp>(planningResource);

                //REFACTOR!!!!!!
                planningApp.Notes = planningResource.Notes;
                planningApp.Developer.FirstName = planningResource.Developer.FirstName;
                planningApp.Developer.LastName = planningResource.Developer.LastName;
                planningApp.Developer.EmailAddress = planningResource.Developer.EmailAddress;
                planningApp.Developer.TelephoneMobile = planningResource.Developer.TelephoneMobile;
                planningApp.Developer.TelephoneWork = planningResource.Developer.TelephoneWork;

                planningApp.DevelopmentAddress.CompanyName = planningResource.DevelopmentAddress.CompanyName;
                planningApp.DevelopmentAddress.AddressLine1 = planningResource.DevelopmentAddress.AddressLine1;
                planningApp.DevelopmentAddress.City = planningResource.DevelopmentAddress.City;
                planningApp.DevelopmentAddress.County = planningResource.DevelopmentAddress.County;
                planningApp.DevelopmentAddress.Postcode = planningResource.DevelopmentAddress.Postcode;
                planningApp.DevelopmentAddress.GeoLocation = planningResource.DevelopmentAddress.GeoLocation;               
                }    
      
            repository.UpdatePlanningApp(planningApp);
            await unitOfWork.CompleteAsync();

            var result = mapper.Map<PlanningApp, PlanningAppResource>(planningApp);
            result.BusinessDate = CurrentDate.SettingDateFormat();

            planningApp = await repository.GetPlanningApp(id, includeRelated: true);
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
    }
}