using vega.Controllers.Resources;
using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppService
    {
        PlanningApp Create(CreatePlanningAppResource planningAppResource);
    }
}