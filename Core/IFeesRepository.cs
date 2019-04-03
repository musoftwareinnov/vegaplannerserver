using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core
{
    public interface IFeeRepository
    {
        List<Fee> GetFeesDefault();
    }
}