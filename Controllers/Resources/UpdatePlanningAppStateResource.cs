using System.Collections.Generic;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class UpdatePlanningAppStateResource
    {
        public int Id { get; set; }
        public bool Reset { get; set; }
        public bool UpdateCustomFieldsOnly { get; set; }
        public string DueByDate { get; set; }
        public ICollection<PlanningAppStateCustomFieldResource> PlanningAppStateCustomFieldsResource { get; set; }
        public string Notes { get; set; }
    }
}