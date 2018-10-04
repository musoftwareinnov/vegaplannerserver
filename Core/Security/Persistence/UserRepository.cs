using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using vega.Persistence;

namespace vegaplanner.Core.Models.Security.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly VegaDbContext vegaDbContext;
        public UserRepository(VegaDbContext vegaDbContext)
        {
            this.vegaDbContext = vegaDbContext;
        }
        public async void Add(InternalAppUser appUser) {
            await vegaDbContext.AppUsers.AddAsync(appUser);
        }

        public async Task<InternalAppUser> Get(Claim userId) {
            return await vegaDbContext.AppUsers.Include(c => c.Identity)
                                        .SingleAsync(c => c.Identity.Id == userId.Value);
        }

        public async Task<List<InternalAppUser>> Get() {

            var users =  await vegaDbContext.AppUsers.Include(c => c.Identity).ToListAsync();

            return users;
        }
    }
}
