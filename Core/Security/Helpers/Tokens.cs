using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using vegaplanner.Core.Models.Security.Auth;
using Newtonsoft.Json;
using vegaplanner.Core.Models.Security.JWT;
using System.Collections.Generic;

namespace vegaplanner.Core.Models.Security.Helpers
{
    public class Tokens
    {
      public static async Task<JwtModel> GenerateJwt(ClaimsIdentity identity, 
                              IJwtFactory jwtFactory,
                              string userName, 
                              JwtIssuerOptions jwtOptions, 
                              JsonSerializerSettings serializerSettings,
                              string roleId
                              )
      {
        var response = new JwtModel
        {
          Id = identity.Claims.Single(c => c.Type == "id"),
          AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity),
          Expiry = (int)jwtOptions.ValidFor.TotalSeconds
        };
        return response;
      }
    }
}