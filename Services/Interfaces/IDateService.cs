using System;
using System.Threading.Tasks;

namespace vega.Services.Interfaces
{
    public interface IDateService
    {
        DateTime GetCurrentDate();
        void SetCurrentDate(DateTime businessDate);
    }
}