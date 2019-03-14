using System.Threading.Tasks;
using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppService
    {
        Task<PlanningApp> Create(CreatePlanningAppResource planningAppResource);
        Task<PlanningApp> InsertGenerator(int planningAppId, int OrderId, int NewGeneratorId);
        Task<PlanningApp> GetPlanningApp(int id);
    }
}