using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using vegaplannerserver.Core.Models;

namespace vega.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public CustomerRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
     
        public async Task<Customer> GetCustomer(int id, bool includeRelated = true)
        { 
            if(includeRelated) {
                var customer = await vegaDbContext.Customers
                                .Include(t => t.planningApps)
                                    .ThenInclude(b => b.CurrentPlanningStatus) 
                                .Include(t => t.planningApps)
                                    .ThenInclude(t => t.PlanningAppStates)
                                        .ThenInclude(s => s.state) 
                                .Include(t => t.planningApps)
                                        .ThenInclude(t => t.PlanningAppStates) 
                                            .ThenInclude(a => a.StateStatus)
                                .SingleOrDefaultAsync(v => v.Id == id);
                
                //Important to keep order of states as they can be added and removed - 
                //EF Core cant do Include(t => t.States.Orderby)

                if(customer.planningApps.Count > 0)
                    customer.planningApps.OrderBy(o => o.Id);
                    
                return customer;
            }
            else {
                return await vegaDbContext.Customers.FindAsync(id);
            }
        } 
        public async Task<QueryResult<Customer>> GetCustomers(CustomerQuery queryObj)
        {
            var result = new QueryResult<Customer>();
            var resList = new List<Customer>();

            var query = vegaDbContext.Customers
                                .OrderBy(c => c.CustomerContact.LastName)
                                .AsQueryable();

            if(queryObj.SearchCriteria != null)
                    query = query.Where(c => c.SearchCriteria.Contains(queryObj.SearchCriteria));

            result.TotalItems =  query.ToList().Count();
            query = query.ApplyPaging(queryObj).Include(pa => pa.planningApps);
            // query = query.Include(pa => pa.planningApps)
            //              .Include(c => c.CustomerContact).

            result.Items = await query.ToListAsync();
            return result;
        }

        public void Add(Customer customer)
        {
            //Store Customer Title for convenience
            customer.CustomerContact.CustomerTitle = vegaDbContext.Title.Where(t => t.Id == customer.CustomerContact.CustomerTitleId).SingleOrDefault().Name;
            vegaDbContext.Add(customer);

        }

        public void Update(Customer customer)
        {
            customer.CustomerContact.CustomerTitle = vegaDbContext.Title.Where(t => t.Id == customer.CustomerContact.CustomerTitleId).SingleOrDefault().Name;
            vegaDbContext.Update(customer);
        }

        public bool CustomerExists(Customer customer)
        {
            return vegaDbContext.Customers
                .Where(fn => fn.CustomerContact.FirstName == customer.CustomerContact.FirstName
                        && fn.CustomerContact.LastName == customer.CustomerContact.LastName).Count() > 0;

        }
    }
}