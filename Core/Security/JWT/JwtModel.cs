using System.Security.Claims;

namespace vegaplanner.Core.Models.Security.JWT
{
    public class JwtModel
    {
        public Claim Id { get; set; }
        public string AuthToken { get; set; }
        public int Expiry { get; set; }
    }
}