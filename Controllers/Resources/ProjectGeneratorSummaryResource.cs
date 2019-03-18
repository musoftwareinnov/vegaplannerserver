using System.Collections.Generic;
using vega.Core.Models.States;

namespace vega.Controllers.Resources
{
    public class ProjectGeneratorSummaryResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectGeneratorSequence> Generators { get; set;}
        public ProjectGeneratorSummaryResource()
        {  
            Generators = new List<ProjectGeneratorSequence>();
        }
    }
}