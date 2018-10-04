using vega.Core.Models.Generic;

namespace vega.Core.Models.States
{
    public class StateStatus : IdNameProperty
    {
        public string GroupType { get; set; }
        public int OrderId { get; set; }
    }
}