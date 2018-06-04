using Dna;
using System.Threading.Tasks;

namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// Handles sending emails specific to the Yhotel
    /// </summary>
    public static class YHotelEmailSender
    {
        /// <summary>
        /// Sends a verification email to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to verify their email</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserVerificationEmailAsync(string displayName, string email, string verificationUrl)
        {
            return await IoC.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
            {
                IsHTML = true,
                FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                ToEmail = email,
                ToName = displayName,
                Subject = "Verify Your Email - Y Hotel"
            },
            "Verify Email The Fuck",
            $"Hi {displayName ?? "stranger"},",
            "Thanks for creating an account with us.<br/>To continue please verify your email with us.",
            "Verify Email",
            verificationUrl
            );
        }


        /// <summary>
        /// Sends a forgot password link to the specified user
        /// </summary>
        /// <param name="displayName">The users display name (typically first name)</param>
        /// <param name="email">The users email to be verified</param>
        /// <param name="verificationUrl">The URL the user needs to click to go for the page of Resetting Password</param>
        /// <returns></returns>
        public static async Task<SendEmailResponse> SendUserForgotPasswordLinkAsync(string displayName, string email, string verificationUrl)
        {
            var a = await IoC.EmailTemplateSender.SendGeneralEmailAsync(new SendEmailDetails
                {
                    IsHTML = true,
                    FromEmail = Framework.Construction.Configuration["EmailSettings:SendEmailFromEmail"],
                    FromName = Framework.Construction.Configuration["EmailSettings:SendEmailFromName"],
                    ToEmail = email,
                    ToName = displayName,
                    Subject = "Forgot Password Link"
                },
                "You're getting old huh? Please drink Memory plus ! ",
                $"Hi {displayName ?? "stranger"},",
                "Kindly remember your password from now on bitch. Thank you",
                "Reset Password",
                verificationUrl
            );

            return a;
        }
    }
}
