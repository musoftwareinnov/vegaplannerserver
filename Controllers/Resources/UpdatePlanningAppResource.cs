using System;
using System.Collections.Generic;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;
using vegaplanner.Controllers.Resources;

namespace vega.Controllers.Resources
{
    public class UpdatePlanningAppResource
    {
        public ContactResource Developer { get; set; }
        public AddressResource DevelopmentAddress { get; set; }
        public ICollection<PlanningAppFeesResource> planningAppFees { get; set; }
        public string Notes { get; set; }
    }
}