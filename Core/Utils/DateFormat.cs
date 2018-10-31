using System;
using Microsoft.Extensions.Options;
using vega.Core.Models.Settings;
using vega.Extensions.DateTime;
using vegaplannerserver.Core;

namespace vega.Core.Utils
{
    public class DateFormatString
    {
        private readonly DateFormatSetting options;
        public DateFormatString(IOptionsSnapshot<DateFormatSetting> options)
        {
            this.options = options.Value;
        }
    }

    public class DateTest
    {
        private readonly DateFormatSetting options;
        public DateTest(IServiceProvider BusinessDateRepository)
        {
            
        }
    }

    public static class DateUtils
    {
            public static DateTime getBusinessDate() {
                //    return DateTime.Now;

                return "10-09-2018".ParseInputDate(); //Get from options somehow!!!!
            }
    }
}