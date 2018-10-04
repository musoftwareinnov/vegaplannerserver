
using vegaplanner.Core.Models.Security.Resources.Validations;
using FluentValidation.Validators;

namespace vegaplanner.Core.Models.Security.Resources
{
    //[Validator(typeof(CredentialsResourceValidator))]
    public class CredentialsResource
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}