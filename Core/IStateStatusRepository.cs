using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Core
{    
    public interface IStateStatusRepository
    {
        Task<List<StateStatus>> GetStateStatusList (string stateStatus);

        List<StateStatus> GetStateStatusListGroup (string stateStatus);

        List<StateStatus> GetStateStatusListCustomer (int CustomerId);
        Task<List<StateStatus>> GetStateStatusList ();
   
        Task<List<StateStatus>> GetStateStatusListInProgress();
        StateStatus GetStateStatus(int id);
    }
}