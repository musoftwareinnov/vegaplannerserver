using System.Collections.Generic;
using vega.Core.Models.States;

namespace vega.Controllers.Resources
{
    public class ProjectGeneratorResource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProjectGeneratorSequence> Generators { get; set;}

        public ProjectGeneratorResource()
        {  
            Generators = new List<ProjectGeneratorSequence>();
        }
    }
}