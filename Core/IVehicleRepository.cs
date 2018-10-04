using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IVehicleRepository
    {
          Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
          void Add(Vehicle Vehicle);

          void Remove(Vehicle Vehicle);

          Task<QueryResult<Vehicle>> GetVehicles(VehicleQuery filter);
    }
}