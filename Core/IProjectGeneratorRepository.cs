using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;
using vega.Core.Models.States;

namespace vega.Core
{
    public interface IProjectGeneratorRepository
    {
        Task<ProjectGenerator> GetProjectGenerator(int id, bool includeRelated = true);
        Task<QueryResult<ProjectGenerator>> GetProjectGenerators();

        void Add(ProjectGenerator projectGenerator);

        void InsertGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator, int SeqId );
    }
}