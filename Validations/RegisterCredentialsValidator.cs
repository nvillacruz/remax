using FluentValidation;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterCredentialsValidator : AbstractValidator<RegisterCredentialsApiModel>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RegisterCredentialsValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty")
                .MinimumLength(8).WithMessage("Password must be greater than 8");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .EmailAddress().WithMessage("Invalid Email");
            RuleFor(x => x.Username)
              .NotEmpty().WithMessage("UserName must not be empty");
            RuleFor(x => x.FirstName)
              .NotEmpty().WithMessage("First Name must not be empty");
            RuleFor(x => x.LastName)
              .NotEmpty().WithMessage("Last Name must not be empty");
        }
    }
}
