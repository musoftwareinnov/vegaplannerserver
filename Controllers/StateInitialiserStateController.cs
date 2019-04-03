using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using vega.Core.Models.States;
using vega.Persistence;
using vega.Controllers.Resources.StateInitialser;
using Microsoft.AspNetCore.Authorization;
using vega.Services.Interfaces;

namespace vega.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("/api/stateinitialiserstates")]
    public class StateInitialiserStateController : Controller
    {
        private readonly IPlanningAppRepository planningAppRepository;
        public StateInitialiserStateController(IMapper mapper,
                                                IStateInitialiserStateRepository repository, 
                                                IPlanningAppRepository planningAppRepository, 
                                                IPlanningAppService planningAppService,
                                                IStateStatusRepository stateStatusRepository,
                                                IUnitOfWork unitOfWork)
        {
            this.planningAppRepository = planningAppRepository;
            this.PlanningAppService = planningAppService;
            this.stateStatusRepository = stateStatusRepository;
            this.mapper = mapper;
            this.repository = repository;
            UnitOfWork = unitOfWork;
        }

        public IMapper mapper { get; }
        public IStateInitialiserStateRepository repository { get; }
        public IPlanningAppService PlanningAppService { get; }
        public IStateStatusRepository stateStatusRepository { get; }
        public IUnitOfWork UnitOfWork { get; }

        [HttpPost]
        public async Task<IActionResult> SaveStateInitialiserState([FromBody] SaveStateInitialiserStateResource stateInitialiserResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (stateInitialiserResource == null)
                return NotFound();

            if (stateInitialiserResource.StateInitialiserId == 0)
            {//Business Validation Check
                ModelState.AddModelError("InitialiserId", "InitialiserId not valid");
                return BadRequest(ModelState);
            }
     
            var stateInitialiserState = mapper.Map<SaveStateInitialiserStateResource, StateInitialiserState>(stateInitialiserResource);

            if (stateInitialiserResource.OrderId == 0)
                repository.AddBeginning(stateInitialiserState);
            else
                repository.AddAfter(stateInitialiserState, stateInitialiserResource.OrderId);

            await UnitOfWork.CompleteAsync();

            var newStateInitialiserState = await repository.GetStateInitialiserState(stateInitialiserState.Id);

            //Get all planning apps that use this state initialiser
            var apps =  planningAppRepository.GetPlanningAppsUsingGenerator(stateInitialiserState.StateInitialiserId, inProgress:true);
 

            foreach(var pa in apps) {
                var genOrderList = planningAppRepository.GetGeneratorOrdersInPlanningApp(pa, stateInitialiserState.StateInitialiserId);
                var current = pa.Current();
                foreach(var genOrder in genOrderList) 
                    PlanningAppService.InsertPlanningState(pa, genOrder, newStateInitialiserState);

                PlanningAppService.UpdateDueByDates(pa);
            }


            await UnitOfWork.CompleteAsync();

            var result = mapper.Map<StateInitialiserState, StateInitialiserStateResource>(newStateInitialiserState);


            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStateInitialiserState(int id)
        {
            var stateInitialiserState = await repository.GetStateInitialiserState(id);

            if (stateInitialiserState == null)
                return NotFound();

            var result = mapper.Map<StateInitialiserState, StateInitialiserStateResource>(stateInitialiserState);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStateInitialiserState([FromBody] StateInitialiserStateResource stateInitialiserStateResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stateInitialiserState = mapper.Map<StateInitialiserStateResource, StateInitialiserState>(stateInitialiserStateResource);
            repository.Update(stateInitialiserState);

            //update states from all current planning applications
            var apps =  planningAppRepository.GetPlanningAppsUsingGenerator(stateInitialiserState.StateInitialiserId, inProgress:true);

            foreach(var pa in apps) {
                PlanningAppService.UpdateDueByDates(pa);
            }
            //apps.ForEach(p => p.updateDueByDates());

            await UnitOfWork.CompleteAsync();

            stateInitialiserState = await repository.GetStateInitialiserState(stateInitialiserState.Id);

            var result = mapper.Map<StateInitialiserState, StateInitialiserStateResource>(stateInitialiserState);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStateInitialiserState(int id)
        {
            var stateInitialiserState = await repository.GetStateInitialiserState(id);

            if (stateInitialiserState == null)
                return NotFound();

            stateInitialiserState.isDeleted = true;

            repository.Update(stateInitialiserState);

            //remove state from all current planning applications
            var apps =  planningAppRepository.GetPlanningAppsUsingGenerator(stateInitialiserState.StateInitialiserId, inProgress:true);

            apps.ForEach(p => p.RemovePlanningState(stateInitialiserState));

            await UnitOfWork.CompleteAsync();

            return Ok(id);
        }
    }
}