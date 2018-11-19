
using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;
using vegaplannerserver.Core.Models;

namespace vegaplannerserver.Core
{
    public interface IDescriptionOfWorkRepository
    {
        Task<QueryResult<DescriptionOfWork>> GetDescriptionsOfWork();
        void Add(DescriptionOfWork descriptionOfWork);

        void Update(DescriptionOfWork descriptionOfWork);
    }
}