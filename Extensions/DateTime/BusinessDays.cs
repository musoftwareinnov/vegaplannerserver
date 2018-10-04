using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using vega.Core.Models.Settings;

namespace vega.Extensions.DateTime
{

    public static class BusinessDays
    {
        public static System.DateTime AddBusinessDays(this System.DateTime source, int businessDays)
        {
            var dayOfWeek = businessDays < 0
                                ? ((int)source.DayOfWeek - 12) % 7
                                : ((int)source.DayOfWeek + 6) % 7;

            switch (dayOfWeek)
            {   
                case 6:
                    businessDays--;
                    break;
                case -6:
                    businessDays++;
                    break;
            }
            return source.AddDays(businessDays + ((businessDays + dayOfWeek) / 5) * 2);
        }

        public static int GetBusinessDays(this System.DateTime current, System.DateTime finishDateExclusive, List<System.DateTime> excludedDates)
        {
            if(System.DateTime.Compare(finishDateExclusive, current ) <= 0)
                return 0;
 
            Func<int, bool> isWorkingDay = days =>
            {
                var currentDate = current.AddDays(days);
                var isNonWorkingDay =
                    currentDate.DayOfWeek == DayOfWeek.Saturday ||
                    currentDate.DayOfWeek == DayOfWeek.Sunday ||
                    excludedDates.Exists(excludedDate => excludedDate.Date.Equals(currentDate.Date));
                return !isNonWorkingDay;
            };

            return Enumerable.Range(0, (finishDateExclusive - current).Days).Count(isWorkingDay);
        }

        public static string SettingDateFormat(this System.DateTime date) {
            return date.ToString("dd-MM-yyyy");
        }

        public static System.DateTime ParseInputDate(this string dateStr) {
           return System.DateTime.ParseExact(dateStr, "dd-MM-yyyy", new CultureInfo("en-US"));
        }    

        // public static System.DateTime CurrentDate(DateSettings dateSettings)
        // {
        //         var date = dateSettings.CurrentDateOverride;
        //         return System.DateTime.ParseExact(dateSettings.CurrentDateOverride, "dd-MM-yyyy", new CultureInfo("en-US"));
        // }
    }
}