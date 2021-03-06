using System;

namespace vega.Core.Utils
{

    public sealed class SystemDate
    {   
        public DateTime date;
        public  Boolean dateOverride = false;
        private static readonly System.Lazy<SystemDate> lazy =
            new Lazy<SystemDate>(() => new SystemDate());

        public static SystemDate Instance { 
            get { 
                    return lazy.Value; 
                }
         }

        private SystemDate()
        {
        }
    }
}