using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using vega.Extensions;
using vega.Core.Models.States;
using Microsoft.Extensions.Options;
using vegaplannerserver.Core;

namespace vega.Persistence
{
    public class BusinessDateRepository : IBusinessDateRepository
    {   
        private readonly VegaDbContext vegaDbContext;

        public BusinessDateRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }

        public DateTime GetBusinessDate()
        {   
            /* TODO:
            ** Future method is to go to a database table and extract a rolled business date
            ** eg,   await vegaDbContext.BusinessDates.SingleOrDefaultAsync();
             */
            //var bd =  await vegaDbContext.BusinessDates.SingleOrDefaultAsync();

            //Currently just get the system date 
            var businessDate = DateTime.Now;
            businessDate = new DateTime(businessDate.Year, businessDate.Month, businessDate.Day, 0, 0, 0);



            Console.WriteLine("Business Date Access : " + businessDate.ToString());

            return businessDate;
        }

        /*
         * Not Curerntly used
         */
        public void SetBusinessDate(DateTime businessDate)
        {   
            //Remove time part 
            businessDate = new DateTime(businessDate.Year, businessDate.Month, businessDate.Day, 0, 0, 0);
            var businessDates =  vegaDbContext.BusinessDates.SingleOrDefault();
            businessDates.CurrBusDate = businessDate;
            
            Console.WriteLine("Business Date set to :" + businessDates.CurrBusDate);
        }
    }
}