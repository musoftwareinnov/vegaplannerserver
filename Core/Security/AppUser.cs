using Microsoft.AspNetCore.Identity;

namespace vegaplanner.Core.Models.Security
{
    public class AppUser: IdentityUser
    {
      // Extended Properties            
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public long? FacebookId { get; set; }  //May Add in future
      public string PictureUrl { get; set; }
    }
}