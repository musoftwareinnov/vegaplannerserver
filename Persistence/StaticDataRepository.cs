using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Persistence;
using vegaplannerserver.Core;
using vegaplannerserver.Core.Models;

namespace vegaplannerserver.Persistence
{
    public class StaticDataRepository : IStaticDataRepository
    {
        public StaticDataRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }

        public VegaDbContext vegaDbContext { get; }

        public async Task<List<Title>> GetTitles() {
            return await vegaDbContext.Title.ToListAsync();
        }
    }
}