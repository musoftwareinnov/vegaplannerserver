

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Core.Models.States;
using vega.Extensions;

namespace vega.Persistence
{
    public class ProjectGeneratorRepository : IProjectGeneratorRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public ProjectGeneratorRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
        public async Task<ProjectGenerator> GetProjectGenerator(int id, bool includeRelated = true)
        {
            if(includeRelated) {
                var projectGenerator = await vegaDbContext.ProjectGenerators
                                                            .Include(pg => pg.Generators)
                                                                //.OrderBy(g => g.SeqId))
                                                            .SingleOrDefaultAsync(g => g.Id == id);

                projectGenerator.Generators = projectGenerator.Generators.ApplySequenceSorting<ProjectGeneratorSequence>();
                return projectGenerator;
            }
            else 
                return await vegaDbContext.ProjectGenerators.FindAsync(id);                  
        }
        public async Task<QueryResult<ProjectGenerator>> GetProjectGenerators() {
            var query = vegaDbContext.ProjectGenerators.Include(pg => pg.Generators);

            var result = new QueryResult<ProjectGenerator>();
            var resList = new List<ProjectGenerator>();

            result.TotalItems =  query.ToList().Count();
            result.Items = await query.ToListAsync();
            return result;
        }

        public void Add(ProjectGenerator projectGenerator) {
            vegaDbContext.Add(projectGenerator);
        }

        //void Update(ProjectGenerator projectGenerator);

        public void InsertGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator, int SeqId) {

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
        public void AppendGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator) {

            ProjectGeneratorSequence pgs = new ProjectGeneratorSequence();
            
            if(projectGenerator.Generators.Count > 0)
                pgs.SeqId = projectGenerator.Generators.Max(g => g.SeqId) + 1; 
            else
                pgs.SeqId = 1;
                
            pgs.Generator = newGenerator;

            projectGenerator.Generators.Add(pgs);          
            vegaDbContext.Update(projectGenerator);
        }


        public void AddGenerator(ProjectGenerator projectGenerator, StateInitialiser newGenerator, StateInitialiser insertAfterGenerator) {

            if(insertAfterGenerator.Id == -1) {
                //Add to front of list
                if(projectGenerator.Generators.Count() == 0) {
                    //projectGenerator.Generators.Add(newGenerator);
                }
                else {
                    ///projectGenerator.Generators.Insert(0, newGenerator);
                }
            }
            else {
                var existingGenerator = projectGenerator.Generators.Where(g => g.Id == insertAfterGenerator.Id);

                //var location = projectGenerator.Generators.IndexOf(insertAfterGenerator);
                //projectGenerator.Generators.Insert(location+1, newGenerator);
            }
            vegaDbContext.Update(projectGenerator);
        }
    }
}