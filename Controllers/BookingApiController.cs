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
    /// Manages the Web API calls about Bookings
    /// </summary>
    public class BookingApiController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application contexta
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
        public BookingApiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }

        #endregion

        public ApiResponse<BookingResultApiModel> CreateBook ([FromBody] BookingCredentialsApiModel model)
        {

            var response = new ApiResponse<BookingResultApiModel>
            {
                ErrorMessage = "Default Error"
            };







            return response;
            }

       
    }
}
