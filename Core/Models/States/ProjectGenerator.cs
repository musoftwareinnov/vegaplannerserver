using System.Collections.Generic;

namespace vega.Core.Models.States
{
    public class ProjectGenerator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StateInitialiser> Generators { get; set; }
    }
}