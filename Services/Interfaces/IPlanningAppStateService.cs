using System;
using vega.Core.Models;

namespace vega.Services.Interfaces
{
    public interface IPlanningAppStateService
    {
         int CompleteState(PlanningAppState planningAppState);

         DateTime SetMinDueByDate(PlanningApp planningApp, PlanningAppState planningAppState);
        void UpdateCustomDueByDate(PlanningAppState planningAppState, DateTime dueByDate);
        bool IsValid(PlanningAppState planningAppState);
    }
}