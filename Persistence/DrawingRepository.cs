using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
    public class DrawingRepository: IDrawingRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public DrawingRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;

        }

        public async Task<IEnumerable<Drawing>> GetDrawings (int planningAppId) {
            return await vegaDbContext.Drawings
                .Where(p => p.PlanningAppId == planningAppId)
                .ToListAsync();
        }       
    }
}