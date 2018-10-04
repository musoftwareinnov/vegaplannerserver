using System.Threading.Tasks;
using vega.Core.Models;
using vega.Persistence;
using vega.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace vega.Persistence
{
    public class PlanningAppStateRepository : IPlanningAppStateRepository
    {
        private readonly VegaDbContext vegaDbContext;
        private readonly IStateInitialiserStateRepository stateRepository;
        public PlanningAppStateRepository(VegaDbContext vegaDbContext, IStateInitialiserStateRepository stateRepository)
        {
            this.stateRepository = stateRepository;
            this.vegaDbContext = vegaDbContext;
        }

        public async Task<PlanningAppState> GetPlanningAppState(int id)
        {

            var appState = vegaDbContext.PlanningAppState.Where(s => s.Id == id)
                                                            .Include(i => i.state)
                                                            .Include(cv => cv.customFields)
                                                           .SingleOrDefault();
            
            appState.state = await stateRepository.GetStateInitialiserState(appState.state.Id);

            return appState;
        }

        public void Update(PlanningAppState planningAppState)
        {
            vegaDbContext.Update(planningAppState);

        }
    }
}