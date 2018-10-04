using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IDrawingRepository    {
         Task<IEnumerable<Drawing>> GetDrawings (int planningAppId);
    }
}