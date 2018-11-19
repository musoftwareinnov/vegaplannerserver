using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using vegaplannerserver.Core;
using vegaplannerserver.Core.Models;

namespace vega.Persistence
{
    public class DescriptionOfWorkRepository : IDescriptionOfWorkRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public DescriptionOfWorkRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
     
        public async Task<QueryResult<DescriptionOfWork>> GetDescriptionsOfWork()
        {
            var result = new QueryResult<DescriptionOfWork>();
            var resList = new List<DescriptionOfWork>();

            var query = vegaDbContext.DescriptionOfWork
                                .OrderBy(c => c.Name)
                                .AsQueryable();

            result.TotalItems =  query.ToList().Count();

            result.Items = await query.ToListAsync();
            return result;
        }

        public void Add(DescriptionOfWork descriptionOfWork)
        {
            vegaDbContext.Add(descriptionOfWork);

        }

        public void Update(DescriptionOfWork descriptionOfWork)
        {
            vegaDbContext.Update(descriptionOfWork);

        }
    }
}