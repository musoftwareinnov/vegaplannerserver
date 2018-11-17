namespace vegaplanner.Core.Models.Security.Helpers
{
    public class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ReadOnlyUser = "readOnlyUser";
                public const string AdminUser = "adminUser";
                public const string CustomerMaintenenceUser = "customerMaintenenceUser";   
                public const string NextStateUser = "nextStateUser";             
            }

            public static class AdminUser {
                public const string Email = "adminuser@gmail.com";
                public const string FirstName = "Admin";
                public const string LastName = "User";
            }
        }
    }
}