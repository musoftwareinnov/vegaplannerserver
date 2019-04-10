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
using vegaplannerserver.Core.Models;
using vegaplanner.Core.Models.Security;

namespace vega.Persistence
{
    public class PlanningAppRepository : IPlanningAppRepository
    {
        private readonly VegaDbContext vegaDbContext;
        private readonly IStateInitialiserRepository stateInitialiserRepository;

        private readonly IStateInitialiserStateRepository stateInitialiserStateRepository;
        private readonly IUserRepository userRepository;

        public PlanningAppRepository(VegaDbContext vegaDbContext, 
                                     IStateStatusRepository stateStatusRepository,
                                     IStateInitialiserRepository stateInitialiserRepository,
                                     IStateInitialiserStateRepository stateInitialiserStateRepository,
                                     IUserRepository userRepository,
                                     IOptionsSnapshot<StateStatusSettings> options)
        {
            this.vegaDbContext = vegaDbContext;
            this.stateStatusRepository = stateStatusRepository;
            this.stateInitialiserRepository = stateInitialiserRepository;
            this.stateInitialiserStateRepository = stateInitialiserStateRepository;
            this.userRepository = userRepository;
            stateStatusSettings = options.Value;
        }

        public StateStatusSettings stateStatusSettings { get; }

        private IStateStatusRepository stateStatusRepository { get; set; }
 
        public void Add(PlanningApp planningApp)
        {
            vegaDbContext.Add(planningApp);   
        }

        public async Task<PlanningApp> GetPlanningApp(int id, bool includeRelated = true)
        {
            if(!includeRelated) {
                return await vegaDbContext.PlanningApps.FindAsync(id);
            }  

            return vegaDbContext.PlanningApps
                    .Where(s => s.Id == id)
                        .Include(b => b.CurrentPlanningStatus)
                        // .Include(p => p.ProjectGenerator)
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
                        .Include(s => s.Surveyors)
                            .ThenInclude(u => u.AppUser)
                        .Include(s => s.Drawers)
                            .ThenInclude(u => u.AppUser)
                        .Include(s => s.Admins)
                            .ThenInclude(u => u.AppUser)
                        .Include(s => s.Fees)
                            .ThenInclude(u => u.Fee)
                        .SingleOrDefault();
  
                       
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

        public QueryResult<PlanningApp> GetPlanningAppsSearchCriteria(PlanningAppQuery queryObj)
        {
            var result = new QueryResult<PlanningApp>();
            var resList = new List<PlanningApp>();

            //Provide sorting list for columns if required
            var columnsMap = new Dictionary<string, Expression<Func<PlanningApp, object>>>()
            {
                ["planningReferenceId"] = r => r.PlanningReferenceId,
                ["descriptionOfWork"] = v => v.DescriptionOfWork,
            };

            var query =  vegaDbContext.PlanningApps
                                .Include(b => b.CurrentPlanningStatus) 
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(a => a.StateStatus)
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(s => s.state)
                                .Include(c => c.Customer.CustomerContact)                           
                                .AsQueryable();

            if(queryObj.PlanningReferenceId != null) {
                query = query.Where(r => r.PlanningReferenceId.Contains(queryObj.PlanningReferenceId));
            }

            query = query.ApplyOrdering(queryObj, columnsMap);
            

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

            var appsInProgress = query.Where(pa => pa.CurrentPlanningStatus.Name == StatusList.AppInProgress);//.ToList();

            //DEBUGGING CHECKS
            foreach(var app in appsInProgress) {
                if(app.Current() == null) {
                    app.Current();
                }
                app.PlanningAppStates = app.OrderedPlanningAppStates.ToList();
            }

            // var ontime = appsInProgress.Where(pa => pa.Current().DynamicStateStatus() == "OnTime")
            //                             .OrderBy(o => o.Current().DueByDate);
            // var due = appsInProgress.Where(pa => pa.Current().DynamicStateStatus() == "Due")
            //                             .OrderBy(o => o.Current().DueByDate);
            // var overdue = appsInProgress.Where(pa => pa.Current().DynamicStateStatus() == "Overdue")
            //                             .OrderBy(o => o.Current().DueByDate);

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
            List<PlanningApp> planningApps = new List<PlanningApp>();
            //Get all planning apps where geenrator id exists
            var qpa = vegaDbContext.PlanningApps
                                .Include(b => b.CurrentPlanningStatus) 
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(a => a.StateStatus)
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(s => s.state)
                                .AsQueryable();
    
            if(inProgress) {
                qpa = qpa.Where(p => p.CurrentPlanningStatus.Name == "InProgress");
                foreach(var pa in qpa) {
                    var curr = pa.Current();
                    if(pa.OrderedPlanningAppStates.Any(p => p.state.StateInitialiserId == generatorId
                                                                    && p.GeneratorOrder >= curr.GeneratorOrder)){
                        planningApps.Add(pa);
                    }

                }
            } 
            return planningApps;
        }

        public HashSet<int> GetGeneratorOrdersInPlanningApp(PlanningApp planningApp, int generatorId)
        {
            //Returns a list of generator orders in the planning app that match generatorId 
            //(Its possible to have the same generator more than once in a planning app)
            HashSet<int> uniqueGenOrders = new HashSet<int>();

            var pa = vegaDbContext.PlanningApps
                                .Where(p => p.Id == planningApp.Id)
                                .Include(b => b.CurrentPlanningStatus) 
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(a => a.StateStatus)
                                .Include(t => t.PlanningAppStates)
                                    .ThenInclude(s => s.state)
                                .SingleOrDefault();
    
            var genOrders = pa.OrderedPlanningAppStates.Where(p => p.state.StateInitialiserId == generatorId);

            foreach(var genOrder in genOrders)
                uniqueGenOrders.Add(genOrder.GeneratorOrder);

            return uniqueGenOrders;
        }

        public PlanningApp UpdatePlanningApp(PlanningApp planningApp)
        {
            vegaDbContext.Update(planningApp);

            return planningApp;
        }
    }
}