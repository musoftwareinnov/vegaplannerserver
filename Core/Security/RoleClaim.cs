using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace vegaplannerserver.Core.Security
{
    public class RoleClaim
    {
        public IdentityRole Role { get; set;}
        public List<Claim> Claims;

        public RoleClaim() {
            Claims = new List<Claim>();
            Role = new IdentityRole();

        }

    }


}