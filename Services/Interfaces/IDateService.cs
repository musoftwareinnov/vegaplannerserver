using System;

namespace vega.Services.Interfaces
{
    public interface IDateService
    {
        DateTime GetCurrentDate();

        void SetCurrentDate(DateTime testDate);
    }
}