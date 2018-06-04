using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json;
using Dna;

namespace Y.Bizz.Web.Server
{

    [Produces("application/json")]
    /// <summary>
    /// Manages the Web API calls
    /// </summary>
    public class ApiController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext mContext;

        /// <summary>
        /// The manager for handling user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;

        /// <summary>
        /// The manager for handling signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        private static readonly HttpClient Client = new HttpClient();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="signInManager">The Identity sign in manager</param>
        /// <param name="userManager">The Identity user manager</param>
        public ApiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }

        #endregion

        /// <su mmary>
        /// Tries to register for a new account on the server
        /// </summary>
        /// <param name="registerCredentials">The registration details</param>
        /// <returns>Returns the result of the register request</returns>
        [Route("api/register")]
        [HttpPost]
        public async Task<ApiResponse<RegisterResultApiModel>> RegisterAsync([FromBody]RegisterCredentialsApiModel registerCredentials)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Please provide all required details to register for an account";

            // The error response for a failed login
            var errorResponse = new ApiResponse<RegisterResultApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            // If we have no credentials...
            if (registerCredentials == null)
                // Return failed response
                return errorResponse;

            // Make sure we have a user name
            if (string.IsNullOrWhiteSpace(registerCredentials.Username))
                // Return error message to user
                return errorResponse;

            // Create the desired user from the given details
            var user = new ApplicationUser
            {
                UserName = registerCredentials.Username,
                FirstName = registerCredentials.FirstName,
                LastName = registerCredentials.LastName,
                Email = registerCredentials.Email
            };

            // Try and create a user
            var result = await mUserManager.CreateAsync(user, registerCredentials.Password);

            // If the registration was successful...
            if (result.Succeeded)
            {
                // Get the user details
                var userIdentity = await mUserManager.FindByNameAsync(registerCredentials.Username);

                // Generate an email verification code
                var emailVerificationCode = await mUserManager.GenerateEmailConfirmationTokenAsync(user);

                // TODO: Replace with APIRoutes that will contain the static routes to use
                var confirmationUrl = $"https://{Request.Host.Value}/api/verify/email/{HttpUtility.UrlEncode(userIdentity.Id)}/{HttpUtility.UrlEncode(emailVerificationCode)}";

                // Email the user the verification code
                await YHotelEmailSender.SendUserVerificationEmailAsync(user.UserName, userIdentity.Email, confirmationUrl);

                // Return valid response containing all users details
                return new ApiResponse<RegisterResultApiModel>
                {
                    Response = new RegisterResultApiModel
                    {
                        FirstName = userIdentity.FirstName,
                        LastName = userIdentity.LastName,
                        Email = userIdentity.Email,
                        Username = userIdentity.UserName,
                        Token = userIdentity.GenerateJwtToken()
                    }
                };
            }
            // Otherwise if it failed...
            else
                // Return the failed response
                return new ApiResponse<RegisterResultApiModel>
                {
                    // Aggregate all errors into a single error string
                    ErrorMessage = result.Errors?.ToList()
                        .Select(f => f.Description)
                        .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
                };
        }

        /// <summary>
        /// Logs in a user using token-based authentication
        /// </summary>
        /// <returns>Returns the result of the login request</returns>
        [Route("api/login")]
        [HttpPost]
        public async Task<ApiResponse<LoginResultApiModel>> LogInAsync([FromBody]LoginCredentialsApiModel loginCredentials)
        {
            // TODO: Localize all strings
            // The message when we fail to login
            var invalidErrorMessage = "Invalid username or password";

            // The error response for a failed login
            var errorResponse = new ApiResponse<LoginResultApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };

            // Make sure we have a user name
            if (string.IsNullOrWhiteSpace(loginCredentials?.UsernameOrEmail))
                // Return error message to user
                return errorResponse;

            // Validate if the user credentials are correct...

            // Is it an email?
            var isEmail = loginCredentials.UsernameOrEmail.Contains("@");

            // Get the user details
            var user = isEmail ?
                // Find by email
                await mUserManager.FindByEmailAsync(loginCredentials.UsernameOrEmail) :
                // Find by username
                await mUserManager.FindByNameAsync(loginCredentials.UsernameOrEmail);

            // If we failed to find a user...
            if (user == null)
                // Return error message to user
                return errorResponse;

            // If we got here we have a user...
            // Let's validate the password

            // Get if password is valid
            var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

            // If the password was wrong
            if (!isValidPassword)
                // Return error message to user
                return errorResponse;

            // If we get here, we are valid and the user passed the correct login details

            // Get username
            var username = user.UserName;

            // Return token to user
            return new ApiResponse<LoginResultApiModel>
            {
                // Pass back the user details and the token
                Response = new LoginResultApiModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    Token = user.GenerateJwtToken()
                }
            };
        }

        /// <summary>
        /// Logs user through facebook account
        /// </summary>
        /// <param name="model">credentials for loggin in</param>
        /// <returns></returns>
        [Route("api/externalauth/facebook")]
        [HttpPost]
        public async Task<ApiResponse<LoginFacebookResultApiModel>> LoginFacebookAsync([FromBody]LoginFacebookCredentialsApiModel model)
        {

            var invalidErrorMessage = "Facebook Login Failed";

            // The error response for a failed login
            var errorResponse = new ApiResponse<LoginFacebookResultApiModel>
            {
                // Set error message
                ErrorMessage = invalidErrorMessage
            };
            // 1.generate an app access token
            var clientId = Framework.Construction.Configuration["FacebookAuth:ClientId"];
            var clientSecret = Framework.Construction.Configuration["FacebookAuth:ClientSecret"];
            var version = Framework.Construction.Configuration["FacebookAuth:Version"];
            var grantType = Framework.Construction.Configuration["FacebookAuth:GrantType"];

            //var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={clientId}&client_secret={clientSecret}&grant_type={grantType}");
            //var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);

            //// 2. validate the user access token
            //var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.Token}&access_token={appAccessToken.AccessToken}");
            //var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            //if (!userAccessTokenValidation.Data.IsValid)
            //    return errorResponse;

            // 3. we've got a valid token so we can request user data from fb
            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/{version}/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.Token}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            // 4. ready to create the local user account (if necessary) and jwt
            var user = await mUserManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new ApplicationUser
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                };

                var result = await mUserManager.CreateAsync(appUser, Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                if (!result.Succeeded)
                    return errorResponse;

                await mContext.Customers.AddAsync(new Customer { Id = 1, Location = "", Locale = userInfo.Locale, Gender = userInfo.Gender });
                await mContext.SaveChangesAsync();
            }

            // generate the jwt for the local user...
            var localUser = await mUserManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
                return errorResponse;

            return new ApiResponse<LoginFacebookResultApiModel>
            {
                Response = new LoginFacebookResultApiModel
                {
                    Token = localUser.GenerateJwtToken()
                }
            };
        }

        // https://stackoverflow.com/questions/37178949/how-do-i-allow-url-encoded-path-segments-in-azure
        [Route("api/verify/email/{userId}/{*emailToken}")]
        [HttpGet]
        public async Task<ActionResult> VerifyEmailAsync(string userId, string emailToken)
        {
            // Get the user
            var user = await mUserManager.FindByIdAsync(userId);

            // NOTE: Issue at the minute with Url Decoding that contains /'s does not replace them
            //       https://github.com/aspnet/Home/issues/2669
            //       
            //       For now, manually fix that
            emailToken = emailToken.Replace("%2f", "/").Replace("%2F", "/");

            // If the user is null
            if (user == null)
                // TODO: Nice UI
                return Content("User not found");

            // If we have the user...

            // Verify the email token
            var result = await mUserManager.ConfirmEmailAsync(user, emailToken);

            // If succeeded...
            if (result.Succeeded)
                // TODO: Nice UI
                return Content("Email Verified :)");

            // TODO: Nice UI
            return Content("Invalid Email Verification Token :(");
        }

        [Route("api/forgot/password")]
        [HttpPost]
        public async Task<ApiResponse<ForgotPasswordResultApiModel>> SendForgotPasswordLinkAsync([FromBody]ForgotPasswordCredentialsApiModel credentials)
        {
            var errorResponse = new ApiResponse<ForgotPasswordResultApiModel>
            {
                ErrorMessage = $"Cannot find {credentials.Email}"
            };

            var user = await mUserManager.FindByEmailAsync(credentials.Email);
            if (user == null || !await mUserManager.IsEmailConfirmedAsync(user))
                return errorResponse;

            var code = await mUserManager.GeneratePasswordResetTokenAsync(user);
            // TODO: Replace with APIRoutes that will contain the static routes to use
            var confirmationUrl = $"https://localhost:3000/reset-password/{HttpUtility.UrlEncode(user.Id)}/{HttpUtility.UrlEncode(code)}";

            //var confirmationUrl = $"https://{Request.Host.Value}/api/reset/password/{HttpUtility.UrlEncode(user.Id)}/{HttpUtility.UrlEncode(code)}";

            var b = await YHotelEmailSender.SendUserForgotPasswordLinkAsync(user.UserName, user.Email, confirmationUrl);

            return new ApiResponse<ForgotPasswordResultApiModel>
            {
                Response = new ForgotPasswordResultApiModel
                {
                    CallBackUrl = confirmationUrl
                }
            };
        }

        [Route("api/sample")]
        [HttpGet]
        public async Task<ActionResult> SampleAsync()
        {
            await Task.Delay(1);
            return Redirect("http://localhost:4200/reset-password");
        }

        [Route("api/reset/password/")]
        [HttpGet]
        public async Task<ActionResult> ResetPasswordAsync(string userId, string code, string newPassword)
        {
            // Get the user
            var user = await mUserManager.FindByIdAsync(userId);

            // NOTE: Issue at the minute with Url Decoding that contains /'s does not replace them
            //       https://github.com/aspnet/Home/issues/2669
            //       
            //       For now, manually fix that
            code = code.Replace("%2f", "/").Replace("%2F", "/");

            //code = HttpUtility.UrlDecode(code);

            // If the user is null
            if (user == null)
                // TODO: Nice UI
                return Content("User not found");

            // If we have the user...

            // Verify the reset code
            var result = await mUserManager.ResetPasswordAsync(user, code, newPassword);


            // If succeeded...
            if (result.Succeeded)
                // TODO: Nice UI
                return Content("Password is reset");

            // TODO: Nice UI
            return Content("Invalid Reset Token :(");
        }

        /// <summary>
        /// Test private area for token-based authentication
        /// </summary>
        /// <returns></returns>
        [AuthorizeToken]
        [Route("api/private")]
        [HttpGet]
        public IActionResult Private()
        {
            // Get the authenticated user
            var user = HttpContext.User;

            // Tell them a secret
            return Ok(new { privateData = $"some secret for {user.Identity.Name}" });
        }


    }
}
