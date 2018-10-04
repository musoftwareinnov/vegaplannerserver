using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using vega.Controllers.Resources.Contact;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class PlanningAppResource
    {
        public int Id { get; set; }
        public string PlanningReferenceId { get; set; }
        public PlanningCustomerResource Customer { get; set; }
        public string Name { get; set; }
        public string BusinessDate { get; set; }
        public string PlanningStatus { get; set; }
        public string CurrentStateStatus { get; set; }
        public string CurrentState { get; set; }
        public string NextState { get; set; }
        public string MinDueByDate { get; set; } //Used for validation when user specifies a completion date
        public string ExpectedStateCompletionDate { get; set; }
        public string CompletionDate { get; set; }
        public string Generator { get; set; }
        public ContactResource Developer { get; set; }
        public Address DevelopmentAddress { get; set; }
        public string Notes { get; set; }

        public ICollection<PlanningAppStateResource> PlanningAppStates { get; set; }

        public PlanningAppResource()
        {
            PlanningAppStates = new Collection<PlanningAppStateResource>();
        }
    }
}