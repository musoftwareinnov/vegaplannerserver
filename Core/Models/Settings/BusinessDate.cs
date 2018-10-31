using System;

namespace vegaplannerserver.Core.Models.Settings
{
    public class BusinessDate
    {
        public int Id { get; set; }
        public DateTime PrevBusDate { get; set; }
        public DateTime CurrBusDate { get; set; } 
        public DateTime NextBusDate { get; set; }     
    }
}