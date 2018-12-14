using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace vegaplanner.Core.Models.Security.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, List<Claim> claimSet);
    }
}