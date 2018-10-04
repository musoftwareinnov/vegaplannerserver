using System.Collections.Generic;
using vega.Core.Models;

namespace vega.Controllers.Resources
{
    public class CustomerResource
    {
        public int Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }
        public string EmailAddress { get; set; }
        public string GeoLocation { get; set; }
        public string TelephoneHome { get; set; }
        public string TelephoneWork{ get; set; }
        public string TelephoneMobile { get; set; }
        public string CustomerAddressSummary { get; set; }
        public string Notes { get; set; }
        public string NameSummary { get; set;}
        public string FullName { get; set;}
        public int planningAppsCount { get; set; }
        
        //public ICollection<PlanningAppSummaryResource> planningApps { get; set; }
    }
}