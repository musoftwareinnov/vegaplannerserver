using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace vegaplanner.Core.Models.Security
{
    public interface IUserRepository
    {
         void Add(InternalAppUser appUser);
         Task<List<InternalAppUser>> Get();

        Task<InternalAppUser> Get(Claim userId);
    }
}