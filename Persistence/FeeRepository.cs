using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vega.Persistence;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
    public class FeeRepository : IFeeRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public FeeRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
        public List<Fee> GetFeesDefault() {
            return vegaDbContext.Fees.ToList();
        }
    }
}