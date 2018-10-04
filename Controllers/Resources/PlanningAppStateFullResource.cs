using System.Collections.Generic;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class PlanningAppStateFullResource
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public string DueByDate { get; set; }
        public string DateCompleted { get; set; }
        public string StateStatus { get; set; }
        public bool CurrentState { get; set; }
        public string MinDueByDate { get; set; }
        public bool DueByDateEditable { get; set; }
        public int PlanningAppId { get; set; }
        public ICollection<PlanningAppStateCustomFieldResource> PlanningAppStateCustomFieldsResource { get; set; }
        public string Notes { get; set; }
    }
}