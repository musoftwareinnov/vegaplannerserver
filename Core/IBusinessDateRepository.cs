using System;
using System.Threading.Tasks;

namespace vegaplannerserver.Core
{
    public interface IBusinessDateRepository
    {
        DateTime GetBusinessDate ();
        void SetBusinessDate (DateTime businessDate );
    }
}