using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vega.Core.Models.Generic;

namespace vega.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }


        public Contact CustomerContact { get; set; }

        public Address CustomerAddress { get; set; }
        public string SearchCriteria { get; set; }

        [StringLength(1024)]
        public string Notes { get; set; }
        public ICollection<PlanningApp> planningApps { get; set; }

        public Customer()
        {
            planningApps = new List<PlanningApp>();
        }
    }
}