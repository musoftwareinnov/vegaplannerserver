using System.Threading.Tasks;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppService
    {
        Task<PlanningApp> Create(CreatePlanningAppResource planningAppResource);
        Task<PlanningApp> InsertGenerator(PlanningApp planningApp, int OrderId, int NewGeneratorId);
        PlanningApp InsertPlanningState(PlanningApp planningApp, int GeneratorOrder, StateInitialiserState stateInitialiserState);
        PlanningApp RemoveGenerator(PlanningApp planningApp, int OrderId, int NewGeneratorId);
        Task<PlanningApp> GetPlanningApp(int id);
        Task<PlanningApp> NextState(PlanningApp planningApp);
        int UpdateDueByDates(PlanningApp planningApp);
    }
}