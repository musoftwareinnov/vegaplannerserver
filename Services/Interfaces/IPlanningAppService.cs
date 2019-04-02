using System.Threading.Tasks;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppService
    {
        Task<PlanningApp> Create(CreatePlanningAppResource planningAppResource);
        Task<PlanningApp> InsertGenerator(PlanningApp planningApp, int OrderId, int NewGeneratorId);
        PlanningApp RemoveGenerator(PlanningApp planningApp, int OrderId, int NewGeneratorId);
        Task<PlanningApp> GetPlanningApp(int id);
        Task<PlanningApp> NextState(PlanningApp planningApp);
        int UpdateDueByDates(PlanningApp planningApp);
    }
}