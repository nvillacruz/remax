using Dna;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Remax.Web.Server
{
    /// <summary>
    /// Manages the Web API calls for Accounts
    /// </summary>
    [Produces("application/json")]
    [AuthorizeToken]
    public class AccountController : Controller
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

        /// <summary>
        /// 
        /// </summary>
        protected RoleManager<IdentityRole> mRoleManager;

        private static readonly HttpClient Client = new HttpClient();

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        /// <param name="signInManager">The Identity sign in manager</param>
        /// <param name="userManager">The Identity user manager</param>
        /// /// <param name="roleManager">The Identity user manager</param>
        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
            mRoleManager = roleManager;

        }

        #endregion

        /// <summary>
        /// Tries to register for a new account on the server
        /// </summary>
        /// <param name="registerCredentials">The registration details</param>
        /// <returns>Returns the result of the register request</returns>
        [Route(ApiRoutes.Register)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<RegisterResultApiModel>> RegisterAsync([FromBody]RegisterCredentialsApiModel registerCredentials)
        {
            var errorResponse = new ApiResponse<RegisterResultApiModel>
            {
                ErrorMessage = UserMessages.UserNotFound
            };

            // Create the desired user from the given details
            var user = new ApplicationUser
            {
                UserName = registerCredentials.Username,
                Email = registerCredentials.Email,
                PhoneNumber = registerCredentials.PhoneNumber,
                Employee = new Employee
                {
                    Id = StringHelper.GenerateNewGuid(),
                    Gender = "male",
                    FirstName = registerCredentials.FirstName,
                    LastName = registerCredentials.LastName,
                    Email = registerCredentials.Email,
                    PhoneNumber = registerCredentials.PhoneNumber,
                }
            };

            //Check if email is unique
            var isNotUnique = await mContext.Users.Where(x => x.NormalizedEmail == registerCredentials.Email.NormalizedUpper()).Select(x => true).SingleOrDefaultAsync();

            if (isNotUnique)
            {
                errorResponse.ErrorMessage = UserMessages.EmailHasBeenTakenAlready;
                return errorResponse;
            }

            // Try and create a user
            var result = await mUserManager.CreateAsync(user, registerCredentials.Password);

            // If the registration was successful...
            if (result.Succeeded)
            {
                // Get the user details
                var userIdentity = await mUserManager.Users.Include(x => x.Employee).Select(x => x).FirstOrDefaultAsync(x => x.UserName == registerCredentials.Username);


                //Add to Employee Role
                await mUserManager.AddToRoleAsync(userIdentity, AspNetRolesDefaults.Employee);

                //Send email verification
                await SendUserEmailVerificationAsync(userIdentity);

                var userRoles = await mUserManager.GetRolesAsync(userIdentity);

                // Return valid response containing all users details
                return new ApiResponse<RegisterResultApiModel>
                {
                    Response = new RegisterResultApiModel
                    {
                        FirstName = userIdentity.Employee.FirstName,
                        LastName = userIdentity.Employee.LastName,
                        Token = userIdentity.GenerateJwtToken(userRoles.ToList()),
                        ImageSource = userIdentity.Employee.ImageSource
                    }
                };
            }
            // Otherwise if it failed...
            else
                // Return the failed response
                return new ApiResponse<RegisterResultApiModel>
                {
                    ErrorMessage = result.Errors.AggregateErrors()
                };
        }

        /// <summary>
        /// Logs in a user using token-based authentication
        /// </summary>
        /// <returns>Returns the result of the login request</returns>
        [Route(ApiRoutes.Login)]
        [HttpPost]
        [ValidateModel]
        [AllowAnonymous]
        public async Task<ApiResponse<LoginResultApiModel>> LogInAsync([FromBody]LoginCredentialsApiModel loginCredentials)
        {
            //Create default error message
            var errorResponse = new ApiResponse<LoginResultApiModel>
            {
                ErrorMessage = UserMessages.InvalidUserNameOrPassword
            };

            // Validate if the user credentials are correct...
            // Is it an email?
            var isEmail = loginCredentials.UsernameOrEmail.Contains("@");
            try
            {
                // Get the user details
                var user = isEmail ?
                    // Find by email
                    await mUserManager.Users
                    .Include(x => x.Employee)
                    .FirstOrDefaultAsync(y => y.Email == loginCredentials.UsernameOrEmail) :
                    // Find by username
                    await mUserManager.Users
                    .Include(x => x.Employee)
                    .FirstOrDefaultAsync(y => y.UserName == loginCredentials.UsernameOrEmail);

                // If we failed to find a user...
                if (user == null)
                    // Return error message to user
                    return errorResponse;

                // If we got here we have a user...
                // Let's validate the password

                // Check if password is valid
                var isValidPassword = await mUserManager.CheckPasswordAsync(user, loginCredentials.Password);

                // If the password was wrong
                if (!isValidPassword)
                    // Return error message to user
                    return errorResponse;

                // Get if the user is confirm
                var isConfirmed = await mUserManager.IsEmailConfirmedAsync(user);
                // if not confirm
                if (!isConfirmed)
                {
                    errorResponse.ErrorMessage = UserMessages.EmailNotConfirmed;
                    // Return error message to user
                    return errorResponse;
                }

                // If we get here, we are valid and the user passed the correct login details
                // Get username
                var username = user.UserName;

                //Get Roles
                var userRoles = await mUserManager.GetRolesAsync(user);

                ////Sign user in with the valid credentials
                //var res = await mSignInManager.PasswordSignInAsync(user, loginCredentials.Password, true, false);

                //if (res.Succeeded)
                //{


                var imageSource = user.Employee.ImageSource == "users\\" ? string.Empty
                    : ConfigurationHelpers.GenerateDefaultUserImagePath(user.Employee.ImageSource);


                // Return token to user
                return new ApiResponse<LoginResultApiModel>
                {
                    // Pass back the user details and the token
                    Response = new LoginResultApiModel
                    {
                        FirstName = user.Employee.FirstName,
                        LastName = user.Employee.LastName,
                        Token = user.GenerateJwtToken(userRoles.ToList()),
                        ImageSource = imageSource,
                    }
                };

            }
            catch (Exception e)
            {
                errorResponse.ErrorMessage = e.InnerException.Message;
            }

            return errorResponse;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route(ApiRoutes.UpdateUser)]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ApiResponse<UpdateUserResultApiModel>> UpdateUserAsync()
        {
            var response = new ApiResponse<UpdateUserResultApiModel>
            {
                ErrorMessage = UserMessages.UserNotFound
            };

            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserID))
                return response;

            try
            {
                var model = new UpdateUserCredentialsApiModel();

                //Get the credentials 
                if (HttpContext.Request.Form.Keys.Count() == 1)
                {
                    var t = await Request.ReadFormAsync();


                    t.Select(x => x.Key).SingleOrDefault();
                    foreach (string key in HttpContext.Request.Form.Keys)
                        if (key.ToLower() == "model")
                            model = JsonConvert.DeserializeObject<UpdateUserCredentialsApiModel>(HttpContext.Request.Form[key]);
                }

                //Get the files to be uploaded
                foreach (var item in Request.Form.Files)
                {
                    if (item.Length > 0)
                    {
                        //Default events path file
                        var filePath = ConfigurationHelpers.GetUserImageDefaultPath();
                        var name = $"e-{Guid.NewGuid()}-{item.FileName}";
                        model.ImageSource = "users\\" + name;
                        filePath = Path.Combine(filePath, name);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }
                    }
                }

                var emailChanged = false;
                var user = await mContext.Users.Include(x => x.Employee).FirstAsync(x => x.Id == currentUserID);
                if (user == null)
                {
                    response.ErrorMessage = UserMessages.UserNotFound;
                    return response;
                }

                user.ConcurrencyStamp = Guid.NewGuid().ToString();

                if (model.PhoneNumber != null)
                    user.PhoneNumber = model.PhoneNumber;
                user.Employee.ImageSource = string.IsNullOrEmpty(model.ImageSource) ? "" : model.ImageSource;


                if (model.FirstName != null)
                    user.Employee.FirstName = model.FirstName;
                if (model.LastName != null)
                    user.Employee.LastName = model.LastName;

                // If we have a email...
                if (model.Email != null &&
                    // And it is not the same...
                    !string.Equals(model.Email.Replace(" ", ""), user.NormalizedEmail))
                {
                    // Update the email
                    user.Email = model.Email;

                    // Un-verify the email
                    user.EmailConfirmed = false;

                    // Flag we have changed email
                    emailChanged = true;
                }

                user.Employee.PhoneNumber = model.PhoneNumber;
                var res = await mUserManager.UpdateAsync(user);
                if (res.Succeeded && emailChanged)
                    await SendUserEmailVerificationAsync(user);

                if (res.Succeeded)
                    return new ApiResponse<UpdateUserResultApiModel>
                    {
                        Response = new UpdateUserResultApiModel
                        {
                            Message = "User Details successfully updated !"
                        }
                    };
                else
                    return new ApiResponse<UpdateUserResultApiModel>
                    {
                        ErrorMessage = res.Errors.AggregateErrors()
                    };
            }
            catch (Exception e)
            {
                // Return the failed response
                return new ApiResponse<UpdateUserResultApiModel>
                {
                    ErrorMessage = e.InnerException.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route(ApiRoutes.UpdateAccountSettings)]
        [HttpPost]
        public async Task<ApiResponse<UpdateAccountSettingResultApiModel>> UpdateAccountSettingAsync([FromBody] UpdateAccountSettingCredentialsApiModel model)
        {
            var response = new ApiResponse<UpdateAccountSettingResultApiModel>
            {
                ErrorMessage = UserMessages.UserNotFound
            };
            var currentUser = await mUserManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
                return response;

            try
            {
                var code = await mUserManager.GeneratePasswordResetTokenAsync(currentUser);

                // Verify the reset code
                var result = await mUserManager.ResetPasswordAsync(currentUser, code, model.Password);

                // If succeeded...
                if (result.Succeeded)
                    return new ApiResponse<UpdateAccountSettingResultApiModel>
                    {
                        Response = new UpdateAccountSettingResultApiModel
                        {
                        }
                    };

                response.ErrorMessage = result.Errors.AggregateErrors();
                return response;
            }
            catch (Exception e)
            {

                // Return the failed response
                return new ApiResponse<UpdateAccountSettingResultApiModel>
                {
                    // Aggregate all errors into a single error string
                    ErrorMessage = e.InnerException.Message
                };
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="emailToken"></param>
        /// <returns></returns>
        [Route(ApiRoutes.VerifyEmail)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<VerifyEmailResultApiModel>> VerifyEmailAsync(string userId, string emailToken)
        {
            // https://stackoverflow.com/questions/37178949/how-do-i-allow-url-encoded-path-segments-in-azure


            var response = new ApiResponse<VerifyEmailResultApiModel>
            {
                Response = new VerifyEmailResultApiModel
                {

                }
            };

            // Get the user
            var user = await mUserManager.FindByIdAsync(userId);

            // NOTE: Issue at the minute with Url Decoding that contains /'s does not replace them
            //       https://github.com/aspnet/Home/issues/2669
            //       
            //       For now, manually fix that
            emailToken = emailToken.Replace("%2f", "/").Replace("%2F", "/");

            // If the user is null
            if (user == null)
                response.ErrorMessage = UserMessages.UserNotFound;

            // Verify the email token
            var result = await mUserManager.ConfirmEmailAsync(user, emailToken);

            // If succeeded...
            if (result.Succeeded)
            {
                response.Response = new VerifyEmailResultApiModel
                {
                    Message = UserMessages.AccountActivated
                };

                return response;
            }

            response.ErrorMessage = UserMessages.InvalidEmailToken;
            // TODO: Nice UI
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [Route(ApiRoutes.SendForgotPasswordLink)]
        [HttpPost]
        [AllowAnonymous]
        [ValidateModel]
        public async Task<ApiResponse<ForgotPasswordResultApiModel>> SendForgotPasswordLinkAsync([FromBody]ForgotPasswordCredentialsApiModel credentials)
        {
            var response = new ApiResponse<ForgotPasswordResultApiModel>
            {
                ErrorMessage = UserMessages.EmailNotRegistered
            };

            var user = await mUserManager.Users.Include(x => x.Employee).FirstOrDefaultAsync(x => x.Email == credentials.Email);
            if (user == null)
            {
                response.ErrorMessage = UserMessages.EmailNotRegistered;
                return response;
            }

            var code = await mUserManager.GeneratePasswordResetTokenAsync(user);
            var confirmationUrl = $"{Framework.Construction.Configuration["Client:Host"]}/reset-password/{HttpUtility.UrlEncode(user.Id)}/{HttpUtility.UrlEncode(code)}";

            var b = await YHotelEmailSender.SendUserForgotPasswordLinkAsync(user.Employee.FirstName, user.Email, confirmationUrl);

            if (b.Successful)
            {
                return new ApiResponse<ForgotPasswordResultApiModel>
                {
                    Response = new ForgotPasswordResultApiModel
                    {
                        Message = UserMessages.PasswordResetMessageSent,
                        CallBackUrl = confirmationUrl
                    }
                };
            }
            else
            {
                return new ApiResponse<ForgotPasswordResultApiModel>
                {
                    ErrorMessage = b.Errors.ToString(),
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [Route(ApiRoutes.ResetPassword)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<ResetPasswordResultApiModel>> ResetPasswordAsync([FromBody]ResetPasswordCredentialsApiModel credentials)
        {

            var errorResponse = new ApiResponse<ResetPasswordResultApiModel>
            {
                ErrorMessage = UserMessages.UserNotFound
            };

            // Get the user
            var user = await mUserManager.FindByIdAsync(credentials.UserId);

            // NOTE: Issue at the minute with Url Decoding that contains /'s does not replace themkd
            //       https://github.com/aspnet/Home/issues/2669
            //       
            //       For now, manually fix that
            var code = credentials.Code.Replace("%2f", "/").Replace("%2F", "/");

            if (user == null)
                return errorResponse;


            // Verify the reset code
            var result = await mUserManager.ResetPasswordAsync(user, code, credentials.NewPassword);

            // If succeeded...
            if (result.Succeeded)
                return new ApiResponse<ResetPasswordResultApiModel>
                {
                    Response = new ResetPasswordResultApiModel
                    {
                        Email = user.Email,
                        Message = UserMessages.PasswordHasBeenReset

                    }
                };

            errorResponse.ErrorMessage = result.Errors?.ToList()
                        .Select(f => f.Description)
                        .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
            return errorResponse;

        }

        #region Get Methods

        /// <summary>
        /// Logs out the user
        /// </summary>
        /// <returns></returns>
        [Route(ApiRoutes.LogOut)]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<LogoutResultApiModel>> LogOutAsync()
        {
            try
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                return new ApiResponse<LogoutResultApiModel>
                {
                    Response = new LogoutResultApiModel
                    {
                        Message = ""
                    }
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<LogoutResultApiModel>
                {
                    ErrorMessage = e.InnerException.Message
                };
            }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route(ApiRoutes.GetCurrentUser)]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ApiResponse<UserProfileResultApiModel>> GetCurrentUser()
        {

            var response = new ApiResponse<UserProfileResultApiModel>
            {
                ErrorMessage = UserMessages.UserNotFound
            };

            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserID))
                return response;

            var user = await mUserManager.Users
                    .Include(x => x.Employee)
                    .FirstOrDefaultAsync(x => x.Id == currentUserID);

            if (user == null)
                return response;

            var phoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? string.Empty : user.PhoneNumber;
 
            var imageSource = user.Employee.ImageSource == "users\\" ? string.Empty
                  : Path.Combine(ConfigurationHelpers.GenerateDefaultUserImagePath(), user.Employee.ImageSource);


            return new ApiResponse<UserProfileResultApiModel>
            {
                // Pass back the user details
                Response = new UserProfileResultApiModel
                {
                    FirstName = user.Employee.FirstName,
                    LastName = user.Employee.LastName,
                    Email = user.Email,
                    Username = user.UserName,
                    PhoneNumber = phoneNumber,
                    ImageSource = imageSource,
                }
            };
        }
        #endregion

        #region Private Methods 

        #endregion

        #region Private Helpers

        /// <summary>
        /// Sends the given user a new verify email link
        /// </summary>
        /// <param name="user">The user to send the link to</param>
        /// <returns></returns>
        private async Task SendUserEmailVerificationAsync(ApplicationUser user)
        {

            // Generate an email verification code
            var emailVerificationCode = await mUserManager.GenerateEmailConfirmationTokenAsync(user);

            //Set the URL of the Api client that will verify the user
            var confirmationUrl = $"{Framework.Construction.Configuration["Client:Host"]}/email-verified/{HttpUtility.UrlEncode(user.Id)}/{HttpUtility.UrlEncode(emailVerificationCode)}";

            // Email the user the verification code
            await YHotelEmailSender.SendUserVerificationEmailAsync(user.Employee.FirstName, user.Email, confirmationUrl);
        }

        #endregion
    }
}
