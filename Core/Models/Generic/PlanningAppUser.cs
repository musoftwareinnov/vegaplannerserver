using vega.Core.Models;
using vegaplanner.Core.Models.Security;

namespace vegaplannerserver.Core.Models.Generic
{
    public class PlanningAppUser
    {
        public int PlanningAppId { get; set; }
        public string AppUserId { get; set; }
        public PlanningApp PlanningApp { get; set; }
        public AppUser AppUser { get; set; }
    }
}