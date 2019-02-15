using vega.Core.Models;

namespace vega.Core.Models.States
{
    public class ProjectGeneratorSequence
    {
        public int Id { get; set; }

        public int SeqId { get; set; }
        public StateInitialiser Generator { get; set; }

        public ProjectGeneratorSequence()
        {  
        }
    }
}