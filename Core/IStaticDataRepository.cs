using System.Collections.Generic;
using System.Threading.Tasks;
using vegaplannerserver.Core.Models;

namespace vega.Core
{
    public interface IStaticDataRepository
    {
        Task<List<Title>> GetTitles();
        string GetTitle(int titleId);
    }
}