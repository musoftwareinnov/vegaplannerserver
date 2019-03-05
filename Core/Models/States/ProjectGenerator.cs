using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using vega.Extensions;

namespace vega.Core.Models.States
{
    public class ProjectGenerator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProjectGeneratorSequence> Generators { get; set;}

        [NotMapped]
        public List<ProjectGeneratorSequence> OrderedGenerators{
            get { return this.Generators.OrderBy(o => o.SeqId).ToList(); } 
        }
        public ProjectGenerator()
        {  
            Generators = new List<ProjectGeneratorSequence>();
        }
    }
}