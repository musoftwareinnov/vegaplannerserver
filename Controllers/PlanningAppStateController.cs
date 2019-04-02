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
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using vega.Services.Interfaces;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/planningappstate")]
    public class PlanningAppStateController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPlanningAppStateRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlanningAppRepository planningAppRepository;

        public PlanningAppStateController(IMapper mapper,
                                     IPlanningAppStateRepository repository,
                                     IPlanningAppRepository planningAppRepository,
                                     IPlanningAppService planningAppService,
                                     IPlanningAppStateService planningAppStateService,
                                     IUnitOfWork unitOfWork)
        {
            this.planningAppRepository = planningAppRepository;
            this.PlanningAppService = planningAppService;
            this.PlanningAppStateService = planningAppStateService;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mapper = mapper;

        }

        public IPlanningAppService PlanningAppService { get; }
        public IPlanningAppStateService PlanningAppStateService { get; }

        [HttpGet("{id}")]
        public async Task<PlanningAppStateFullResource> GetPlanningAppState(int id)
        {
            var planningAppState = await repository.GetPlanningAppState(id);
            var planningApp = await planningAppRepository.GetPlanningApp(planningAppState.PlanningAppId);
            var planningAppStateResource = mapper.Map<PlanningAppState, PlanningAppStateFullResource>(planningAppState);

            //populate the custom fields with values set in 'customStateValue'
            foreach( var customFieldResource in planningAppStateResource.PlanningAppStateCustomFieldsResource) {
                var customField = planningAppState.getPlanningAppStateCustomField(customFieldResource.Id);
                if(customField != null) 
                    customFieldResource.Value = planningAppState.getPlanningAppStateCustomField(customFieldResource.Id).StrValue;
            }

            //If a live state set the min due by date
            planningAppStateResource.DueByDateEditable = false;
            planningAppStateResource.MinDueByDate = DateTime.Now.Date.ToString();  //TODO: Remove when fix front end
            if(!planningAppState.Completed()) {
                DateTime minDueDate = PlanningAppStateService.SetMinDueByDate(planningApp, planningAppState);
                planningAppStateResource.MinDueByDate = minDueDate.SettingDateFormat();
                planningAppStateResource.DueByDateEditable = minDueDate > SystemDate.Instance.date;
            }
            return planningAppStateResource;
        }


        [HttpPut("{id}")]
        public async Task<PlanningAppStateFullResource> UpdatePlanningAppState(int id, [FromBody] UpdatePlanningAppStateResource planningAppStateResource)
        {
            var planningAppState = await repository.GetPlanningAppState(id);
            var planningApp = await planningAppRepository.GetPlanningApp(planningAppState.PlanningAppId);
            var dueByDate = planningAppStateResource.DueByDate.ParseInputDate();
            
            if(dueByDate != planningAppState.DueByDate) {
                PlanningAppStateService.UpdateCustomDueByDate(planningAppState, dueByDate);
                PlanningAppService.UpdateDueByDates(planningApp); //Updates all forward dueby dates from current position 
            }

            //Set any fields in the PlanningApp table that have been set in the Rule List
            if(planningAppStateResource.PlanningAppStateCustomFieldsResource.Count() > 0) {
                foreach (var customStateValueResource in planningAppStateResource.PlanningAppStateCustomFieldsResource) 
                    planningAppState.getPlanningAppStateCustomField(customStateValueResource.Id).StrValue = customStateValueResource.Value;

                //Store custom fields in planning state
                planningApp.UpdateKeyFields(planningAppStateResource.PlanningAppStateCustomFieldsResource);
            }
            planningAppState.Notes = planningAppStateResource.Notes;
            repository.Update(planningAppState);
            await unitOfWork.CompleteAsync();

            return await GetPlanningAppState(id);
        }
    }
}