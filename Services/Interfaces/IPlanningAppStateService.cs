using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppStateService
    {
         int CompleteState(PlanningAppState planningAppState);
         bool IsValid(PlanningAppState planningAppState);
    }
}