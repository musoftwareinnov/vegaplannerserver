using FluentValidation;
using vegaplanner.Core.Models.Security.Resources;

namespace vegaplanner.Core.Models.Security.Resources.Validations
{
    public class CredentialsResourceValidator : AbstractValidator<CredentialsResource>
    {
        public CredentialsResourceValidator()
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
        }
    }
}