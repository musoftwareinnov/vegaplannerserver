using vega.Core.Models;
using vegaplanner.Core.Models.Security;

namespace vegaplannerserver.Core.Models.Generic
{
    public class PlanningAppUser
    {
        public int PlanningAppId { get; set; }
        public int InternalAppUserId { get; set; }
        public PlanningApp PlanningApp { get; set; }
        public InternalAppUser InternalAppUser { get; set; }
    }
}