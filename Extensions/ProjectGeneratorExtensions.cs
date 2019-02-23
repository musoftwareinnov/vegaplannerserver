using vega.Core.Models.States;
using vega.Extensions;

namespace vegaplannerserver.Extensions
{
    public static class ProjectGeneratorExtensions
    {
        public static ProjectGenerator sort(this ProjectGenerator projectGenerator) 
                                                                    
        {
            projectGenerator.Generators = projectGenerator.Generators.ApplySequenceSorting();
            projectGenerator.Generators.ForEach(g => g.Generator.States.ApplyStateSequenceSorting());
            return projectGenerator;
        }
    }
}