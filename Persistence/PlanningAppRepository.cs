using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using vega.Extensions;
using vega.Core.Models.States;
using Microsoft.Extensions.Options;

namespace vega.Persistence
{
    public class PlanningAppRepository : IPlanningAppRepository
    {
        private readonly VegaDbContext vegaDbContext;
        private readonly IStateInitialiserRepository stateInitialiserRepository;

        private readonly IStateInitialiserStateRepository stateInitialiserStateRepository;

        public PlanningAppRepository(VegaDbContext vegaDbContext, 
                                     IStateStatusRepository stateStatusRepository,
                                     IStateInitialiserRepository stateInitialiserRepository,
                                     IStateInitialiserStateRepository stateInitialiserStateRepository,
                                     IOptionsSnapshot<StateStatusSettings> options)
        {
            this.vegaDbContext = vegaDbContext;
            this.stateStatusRepository = stateStatusRepository;
            this.stateInitialiserRepository = stateInitialiserRepository;
            this.stateInitialiserStateRepository = stateInitialiserStateRepository;
            stateStatusSettings = options.Value;
        }

        public StateStatusSettings stateStatusSettings { get; }

        private IStateStatusRepository stateStatusRepository { get; set; }
 
        public void Add(PlanningApp planningApp, StateInitialiser stateInitialiser)
        {

            var initialStatus = vegaDbContext.StateStatus.Where(s => s.Name == stateStatusSettings.STATE_ON_TIME).SingleOrDefault();
            var initialStatusList  = vegaDbContext.StateStatus.ToList();
            
            planningApp = planningApp.GeneratePlanningStates(stateInitialiser.States, initialStatusList);
            vegaDbContext.Add(planningApp);   
        }

        public async Task<PlanningApp> GetPlanningApp(int id, bool includeRelated = true)
        {
            if(!includeRelated) {
                return await vegaDbContext.PlanningApps.FindAsync(id);
            }  
            else {
                var sortStates =  vegaDbContext.PlanningApps
                                .Where(s => s.Id == id)
                                    .Include(b => b.CurrentPlanningStatus)
                                    .Include(t => t.PlanningAppStates)
                                        .ThenInclude(s => s.state) 
                                            .ThenInclude(cs => cs.StateInitialiserStateCustomFields)
                                                .ThenInclude(cf => cf.StateInitialiserCustomField)
                                    .Include(t => t.PlanningAppStates)
                                        .ThenInclude(a => a.StateStatus)
                                    .Include(t => t.PlanningAppStates)
                                        .ThenInclude(cf => cf.customFields)
                                    .Include(c => c.Customer.CustomerContact)
                                    .Include(c => c.Customer.CustomerAddress)
                                    .Include(g => g.StateInitialiser)
                                    .SingleOrDefault();

                //sort planing states using 
                if(sortStates != null)
                    sortStates.PlanningAppStates = sortStates.PlanningAppStates.OrderBy(o => o.state.OrderId).ToList();

                return sortStates;
            }            
        }

        public QueryResult<PlanningApp> GetPlanningApps(PlanningAppQuery queryObj)
        {
            var result = new QueryResult<PlanningApp>();
            var resList = new List<PlanningApp>();

            var query =  vegaDbContext.PlanningApps
                                .Include(b => b.CurrentPlanningStatus) 
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(a => a.StateStatus)
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(s => s.state)
                                .Include(c => c.Customer.CustomerContact)
                                .AsQueryable();

            if(queryObj.CustomerId > 0)
                query = query.Where(c => c.Customer.Id == queryObj.CustomerId);
 
            if(queryObj.PlanningAppType==null) {
                queryObj.PlanningAppType = StatusList.AppInProgress;
            }

            //Build up list of planning apps
            List<PlanningApp> planningAppSelectList = new List<PlanningApp>();

            if(queryObj.PlanningAppType == StatusList.All ) {
                planningAppSelectList = getAppsInProgress(query);   
                planningAppSelectList.AddRange(getAppsNotInProgress(query));
            }
            else if(queryObj.PlanningAppType == StatusList.AppInProgress ) {
                planningAppSelectList = getAppsInProgress(query);        
            }
            else if(queryObj.PlanningAppType == StatusList.AppNotInProgress ) { //ie, Completed/Archived/Terminated
                planningAppSelectList = getAppsNotInProgress(query);
            }   
            else 
                planningAppSelectList = getAppsWithStatus(query, queryObj.PlanningAppType); //Individual state selected       

            query = planningAppSelectList.AsQueryable();
            result.TotalItems =  query.Count();
            query = query.ApplyPaging(queryObj); 
            result.Items = query.ToList();
            return result;
        }

        public List<PlanningApp> getAppsWithStatus(IQueryable<PlanningApp> query, string planningAppType) {

            var statusListInProgress = stateStatusRepository.GetStateStatusListGroup(StatusList.AppInProgress);
            var statusListNotInProgress = stateStatusRepository.GetStateStatusListGroup(StatusList.AppNotInProgress);
            List<PlanningApp> planningAppSelectList = new List<PlanningApp>();

            if(statusListInProgress.Exists(s => s.Name == planningAppType)) {
                var appsInProgress = query.Where(pa => pa.CurrentPlanningStatus.Name == StatusList.AppInProgress).ToList();
                planningAppSelectList.AddRange(appsInProgress.Where(pa => pa.Current().DynamicStateStatus() == planningAppType)
                    .OrderBy(o => o.Current().DueByDate));
            }
            else if(statusListNotInProgress.Exists(s => s.Name == planningAppType)) {
                planningAppSelectList.AddRange(query.Where(pa => pa.CurrentPlanningStatus.Name == planningAppType)
                                        .OrderByDescending(o => o.Id));
            }
            return planningAppSelectList;
        }

        public List<PlanningApp> getAppsInProgress(IQueryable<PlanningApp> query) {

            List<PlanningApp> planningAppSelectList = new List<PlanningApp>();
            var statusListInProgress = stateStatusRepository.GetStateStatusListGroup(StatusList.AppInProgress);

            var appsInProgress = query.Where(pa => pa.CurrentPlanningStatus.Name == StatusList.AppInProgress).ToList();

            foreach(var app in appsInProgress) {
                app.PlanningAppStates = app.PlanningAppStates.OrderBy(o => o.state.OrderId).ToList();
            }
            foreach(var status in statusListInProgress) { 
                planningAppSelectList.AddRange(appsInProgress.Where(pa => pa.Current().DynamicStateStatus() == status.Name)
                                        .OrderBy(o => o.Current().DueByDate));
            }  

            return planningAppSelectList;
        }

        public List<PlanningApp> getAppsNotInProgress(IQueryable<PlanningApp> query) {
            var statusListNotInProgress = stateStatusRepository.GetStateStatusListGroup(StatusList.AppNotInProgress);
            List<PlanningApp> planningAppSelectList = new List<PlanningApp>();
            foreach(var status in statusListNotInProgress) { 
                    planningAppSelectList.AddRange(query.Where(pa => pa.CurrentPlanningStatus.Name == status.Name)
                                        .OrderByDescending(o => o.Id));
            } 
            return planningAppSelectList;
        }

        public List<PlanningApp> GetPlanningAppsUsingGenerator(int generatorId, bool inProgress = true)
        {
            return  vegaDbContext.PlanningApps
                                .Where(p => p.StateInitialiserId == generatorId && p.CurrentPlanningStatus.Name == "InProgress")
                                .Include(b => b.CurrentPlanningStatus) 
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(a => a.StateStatus)
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(s => s.state)
                                .ToList();
        }

        public PlanningApp UpdatePlanningApp(PlanningApp planningApp)
        {
            vegaDbContext.Update(planningApp);

            return planningApp;
        }
    }
}