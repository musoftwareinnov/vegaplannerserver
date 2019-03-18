using System.Collections.Generic;
using vega.Core.Models.States;

namespace vega.Controllers.Resources
{
    public class ProjectGeneratorCreateResource
    {  
        public string Name { get; set; }
        public List<ProjectGeneratorSequence> Generators { get; set;}
        public ProjectGeneratorCreateResource()
        {  
            Generators = new List<ProjectGeneratorSequence>();
        }
    }
}