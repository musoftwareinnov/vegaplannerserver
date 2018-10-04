using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IStateInitialiserRepository
    {
        Task<StateInitialiser> GetStateInitialiser(int id, bool includeDeleted=false);
        Task<QueryResult<StateInitialiser>>  GetStateInitialisers(StateInitialiserQuery queryObj);
        void Add(StateInitialiser StateInitialiser);

        //   void Remove(StateInitialiser StateInitialiser);
    }
}