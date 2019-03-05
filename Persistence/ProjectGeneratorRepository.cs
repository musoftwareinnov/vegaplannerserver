

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Extensions;
using vegaplannerserver.Extensions;

namespace vega.Persistence
{
    public class ProjectGeneratorRepository : IProjectGeneratorRepository
    {
        private readonly VegaDbContext vegaDbContext;
        private readonly IStateInitialiserRepository StateInitialiserRepository;
        public ProjectGeneratorRepository(VegaDbContext vegaDbContext, IStateInitialiserRepository StateInitialiserRepository )
        {
            this.StateInitialiserRepository = StateInitialiserRepository;
            this.vegaDbContext = vegaDbContext;
        }
        public async Task<ProjectGenerator> GetProjectGenerator(int id, bool includeRelated = true)
        {
            if (includeRelated)
            {
                var projectGenerator = await vegaDbContext.ProjectGenerators
                                                            .Include(pg => pg.Generators)
                                                                .ThenInclude(gn => gn.Generator)
                                                                    .ThenInclude(st => st.States)
                                                            .SingleOrDefaultAsync(g => g.Id == id);
                return projectGenerator;
            }
            else
                return await vegaDbContext.ProjectGenerators.FindAsync(id);
        }
        public async Task<QueryResult<ProjectGenerator>> GetProjectGenerators()
        {
            var query = vegaDbContext.ProjectGenerators.Include(pg => pg.Generators);

            var result = new QueryResult<ProjectGenerator>();
            var resList = new List<ProjectGenerator>();

            result.TotalItems = query.ToList().Count();
            result.Items = await query.ToListAsync();
            return result;
        }

        public void Add(ProjectGenerator projectGenerator)
        {
            vegaDbContext.Add(projectGenerator);
        }

        public void InsertGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator, int SeqId)
        {

            ProjectGeneratorSequence pgs = new ProjectGeneratorSequence();

            pgs.SeqId = SeqId;
            pgs.Generator = newGenerator;

            projectGenerator.Generators
                .Where(g => g.SeqId >= SeqId)
                .ToList()
                .ForEach(gn => gn.SeqId += 1);

            projectGenerator.Generators.Add(pgs);

            vegaDbContext.Update(projectGenerator);
        }

        public async void RemoveGenerator(ProjectGenerator projectGenerator, StateInitialiser generator)
        {

            var pg = await GetProjectGenerator(projectGenerator.Id);

            var rg = pg.Generators.Where(g => g.Generator.Id == generator.Id).SingleOrDefault();

            var SeqId = rg.SeqId; //Store Sequence
            pg.Generators.Remove(rg);

            projectGenerator.Generators
                .Where(g => g.SeqId >= SeqId)
                .ToList()
                .ForEach(gn => gn.SeqId -= 1);

            vegaDbContext.Update(projectGenerator);
        }
        public void AppendGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator)
        {

            ProjectGeneratorSequence pgs = new ProjectGeneratorSequence();

            if (projectGenerator.Generators.Count > 0)
                pgs.SeqId = projectGenerator.Generators.Max(g => g.SeqId) + 1;
            else
                pgs.SeqId = 1;

            pgs.Generator = newGenerator;

            projectGenerator.Generators.Add(pgs);
            vegaDbContext.Update(projectGenerator);
        }
    }
}