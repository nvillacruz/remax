using FluentValidation;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginCredentialsValidator: AbstractValidator<LoginCredentialsApiModel>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoginCredentialsValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must not be empty")
                .MinimumLength(6).WithMessage("Password must be greater than 6");
            RuleFor(x => x.UsernameOrEmail)
                .NotEmpty().WithMessage("Email must not be empty")
                .EmailAddress().WithMessage("Invalid Email");
        }
    }
}
