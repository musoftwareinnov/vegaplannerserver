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
using vegaplannerserver.Core.Models.Settings;
using vega.Core.Utils;

namespace vega.Persistence
{
    public class BusinessDateRepository : IBusinessDateRepository
    {   
        private readonly VegaDbContext vegaDbContext;

        public BusinessDateRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }

        public async Task<BusinessDate> GetBusinessDate()
        {   
                return await vegaDbContext.BusinessDates.SingleOrDefaultAsync();
        }

        public void SetBusinessDate(DateTime businessDate)
        {   
            foreach (var entity in vegaDbContext.BusinessDates)
                vegaDbContext.BusinessDates.Remove(entity);     
     
            BusinessDate currBusinessDate = new BusinessDate();
            currBusinessDate.CurrBusDate = businessDate;
            currBusinessDate.PrevBusDate = businessDate;
            currBusinessDate.NextBusDate = businessDate;
            this.vegaDbContext.Add(currBusinessDate);

            //Console.WriteLine("Business Date set to :" + currBusinessDate.CurrBusDate);
            this.vegaDbContext.SaveChanges();  //Anti Pattern - should use UnitOfWork (one off! :-)

        }
    }
}