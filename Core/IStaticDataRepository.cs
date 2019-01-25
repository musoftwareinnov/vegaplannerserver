using System.Collections.Generic;
using System.Threading.Tasks;
using vegaplannerserver.Core.Models;

namespace vegaplannerserver.Core
{
    public interface IStaticDataRepository
    {
        Task<List<Title>> GetTitles();
    }
}