using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace vegaplanner.Core.Models.Security.Resources
{

    public class RegistrationResource
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<string> Roles { get; set; }
        public string Location { get; set; }

        public RegistrationResource()
        {
            Roles = new Collection<String>();
        }
    }



}