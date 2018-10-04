using System.Collections.Generic;

namespace vega.Controllers.Resources.StateInitialser
{
    public class StateInitialiserResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public IList<StateInitialiserStateResource> States { get; set; }

        public StateInitialiserResource()
        {
            States = new List<StateInitialiserStateResource>();
        }
    }
}