using System;
using vega.Services.Interfaces;

namespace vega.Services.Implementations
{
    public class DateService : IDateService
    {
        public DateTime GetCurrentDate() {

            var d = DateTime.Now.Date;
            return DateTime.Now.Date;
        }

        public void SetCurrentDate(DateTime testDate) {
            //Test Date - only used for testing (override)
        }
    }
}