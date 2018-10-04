using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Persistence
{
    public class StateStatusRepository : IStateStatusRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public StateStatusRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;

        }
  
        public async Task<List<StateStatus>> GetStateStatusList (string stateStatus)
        {
             if(stateStatus == StatusList.All) {
                return vegaDbContext.StateStatus.Where(s => s.GroupType != s.Name).OrderBy(o => o.OrderId).ToList();
            }           
            if(stateStatus == null) 
                return await GetStateStatusList();
            else {        
                var status = vegaDbContext.StateStatus.Where(s => s.Name == stateStatus).SingleOrDefault();
                
                if(status.Name == status.GroupType)
                    return await vegaDbContext.StateStatus.Where(s => s.GroupType == stateStatus).OrderBy(o => o.OrderId).ToListAsync();
                else
                    return await GetStateStatusList();
            }
        } 

        public List<StateStatus> GetStateStatusListGroup (string stateStatus)
        {     
            if(stateStatus == StatusList.All) {
                return vegaDbContext.StateStatus.Where(s => s.GroupType != s.Name).OrderBy(o => o.OrderId).ToList();
            }
            var statusList = vegaDbContext.StateStatus.Where(s => s.GroupType == stateStatus).OrderBy(o => o.OrderId).ToList();
            if(statusList.Count() > 0 ) //We have a group selection
                statusList.Remove(statusList.Where(s => s.Name ==stateStatus).SingleOrDefault()); //Remove group status
            else {
                //not a group status single only
                 statusList = vegaDbContext.StateStatus.Where(s => s.Name == stateStatus).OrderBy(o => o.OrderId).ToList();
            }
            return statusList;          
        } 

        public List<StateStatus> GetStateStatusListCustomer (int customerId)
        {     
            var stateStatus = "OnTime";
            // if(stateStatus == StatusList.All) {
            //     return  vegaDbContext.StateStatus.Where(s => s.GroupType != s.Name).OrderBy(o => o.OrderId).ToList();
            // }
            var statusList = vegaDbContext.StateStatus.Where(s => s.GroupType == stateStatus).OrderBy(o => o.OrderId).ToList();
            if(statusList.Count() > 0 ) //We have a group selection
                statusList.Remove(statusList.Where(s => s.Name ==stateStatus).SingleOrDefault()); //Remove group status
            else {
                //not a group status single only
                 statusList = vegaDbContext.StateStatus.Where(s => s.Name == stateStatus).OrderBy(o => o.OrderId).ToList();
            }
            return  statusList;          
        }

        public async Task<List<StateStatus>> GetStateStatusList ()
        {
            return await vegaDbContext.StateStatus.OrderBy(o => o.OrderId).ToListAsync();
        } 

        public async Task<List<StateStatus>> GetStateStatusListInProgress ()
        {
            var inProgress = vegaDbContext.StateStatus.AsQueryable();
            
            inProgress.Where(s => s.Name == "OnTime");

            return await inProgress.ToListAsync();
        } 

        public StateStatus GetStateStatus(int id)
        {
            return vegaDbContext.StateStatus.Where(s => s.Id == id).SingleOrDefault();
           
        } 
    }
}