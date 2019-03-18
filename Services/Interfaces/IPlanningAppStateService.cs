using System;
using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppStateService
    {
         int CompleteState(PlanningAppState planningAppState);

         DateTime SetMinDueByDate(PlanningApp planningApp, PlanningAppState planningAppState);
         bool IsValid(PlanningAppState planningAppState);
    }
}