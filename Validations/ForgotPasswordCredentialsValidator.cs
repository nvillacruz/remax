using FluentValidation;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ForgotPasswordCredentialsValidator : AbstractValidator<ForgotPasswordCredentialsApiModel>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ForgotPasswordCredentialsValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Password must not be empty");
               
        }
    }
}
