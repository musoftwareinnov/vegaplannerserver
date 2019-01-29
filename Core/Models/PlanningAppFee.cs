using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using vega.Core.Models;
using vega.Core.Models.Generic;

namespace vegaplannerserver.Core.Models
{
    public class PlanningAppFees
    {
        public int PlanningAppId { get; set; }
        public int FeeId { get; set; }
        public PlanningApp PlanningApp { get; set; }
        public Fee Fee { get; set; }
        public decimal Amount { get; set; }
    }
}