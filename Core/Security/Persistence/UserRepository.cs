using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using vegaplanner.Core.Models.Security.Helpers;
using vega.Core;
using vega.Core.Models;
using vega.Extensions;
using vega.Persistence;
using System;

namespace vegaplanner.Core.Models.Security.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly VegaDbContext vegaDbContext;
        private readonly UserManager<AppUser> userManager;
        public UserRepository(VegaDbContext vegaDbContext, UserManager<AppUser> userManager)
        {
            this.vegaDbContext = vegaDbContext;
            this.userManager = userManager;
            
 
        }
        public async void Add(InternalAppUser appUser) {
            await vegaDbContext.AppUsers.AddAsync(appUser);
        }

        public async Task<InternalAppUser> Get(Claim userId) {
            return await vegaDbContext.AppUsers.Include(c => c.Identity)
                                        .SingleAsync(c => c.Identity.Id == userId.Value);
        }

        public async Task<IList<AppUser>> GetUsers(string role) {
            var surveyors = new List<AppUser>();
            var users = userManager.GetUsersInRoleAsync(role)   ;                 

            return await users;
        }

        public InternalAppUser GetByInternalId(int internalUserId) {
            return vegaDbContext.AppUsers.Include(c => c.Identity)
                                        .Single(c => c.Id == internalUserId);
        }

        public async Task<List<InternalAppUser>> Get() {

            var users =  await vegaDbContext.AppUsers.Include(c => c.Identity).ToListAsync();

            return users;
        }
    }
}
