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
    public class StateInitialiserRepository : IStateInitialiserRepository
    {   
            private readonly VegaDbContext vegaDbContext;
        public StateInitialiserRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }

        public async Task<StateInitialiser> GetStateInitialiser(int id, bool includeDeleted=false)
        {   
            var stateInitialiser =  await vegaDbContext.StateInitialisers
                            .Where(s => s.Id == id)
                                .Include(t => t.States)
                                    .ThenInclude(s => s.StateInitialiserStateCustomFields)   
                                        .ThenInclude(r => r.StateInitialiserCustomField)         
                                .SingleOrDefaultAsync();
        
            IOrderedEnumerable<StateInitialiserState> orderedStates;
            if(includeDeleted) 
                orderedStates =  stateInitialiser.States.OrderBy(o => o.OrderId);
            else 
                orderedStates =  stateInitialiser.States.Where(s => s.isDeleted == includeDeleted).OrderBy(o => o.OrderId);

            stateInitialiser.States = orderedStates.ToList();

            return stateInitialiser;
        }

        public async Task<QueryResult<StateInitialiser>>  GetStateInitialisers(StateInitialiserQuery queryObj)
        {
            var result = new QueryResult<StateInitialiser>();

            var query = vegaDbContext.StateInitialisers.AsQueryable();

            result.TotalItems =  query.Count();
            query = query.ApplyPaging(queryObj);

            result.Items = await query.ToListAsync();

            return result;          
        }

        public void Add(StateInitialiser stateInitialiser)
        {
            vegaDbContext.Add(stateInitialiser);

        }
    }
}
   