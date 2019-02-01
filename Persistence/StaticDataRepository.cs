using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Persistence;
using vegaplannerserver.Core;
using vegaplannerserver.Core.Models;

namespace vega.Persistence
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
        public string GetTitle(int titleId) {
            return vegaDbContext.Title.Where(t => t.Id == titleId).SingleOrDefault().Name;
        }
    }
}