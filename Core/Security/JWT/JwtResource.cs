namespace vegaplanner.Core.Models.Security.JWT
{
    public class JwtResource
    {
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public int Expiry { get; set; }
        public string UserName { get; set; }
    }
}