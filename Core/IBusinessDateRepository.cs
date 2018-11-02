using System;
using System.Threading.Tasks;
using vegaplannerserver.Core.Models.Settings;

namespace vegaplannerserver.Core
{
    public interface IBusinessDateRepository
    {
        Task<BusinessDate> GetBusinessDate();
        void SetBusinessDate (DateTime businessDate );

    }
}