using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public PhotoRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;

        }

        public async Task<IEnumerable<Photo>> GetPhotos (int vehicleId) {
            return await vegaDbContext.Photos
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }       
    }
}