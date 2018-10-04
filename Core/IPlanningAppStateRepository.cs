using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IPlanningAppStateRepository
    {

        Task<PlanningAppState> GetPlanningAppState(int id);
        void Update(PlanningAppState planningAppState);
    }
}