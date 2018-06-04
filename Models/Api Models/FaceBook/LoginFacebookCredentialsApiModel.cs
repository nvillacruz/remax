namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// The credentials of a facebook login request via API
    /// </summaryLoginFacebookResultApiModel
    public class LoginFacebookCredentialsApiModel
    {
        #region Public Properties

        /// <summary>
        /// The authentication token used to stay authenticated through future requests
        /// </summary>
        public string Token { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginFacebookCredentialsApiModel()
        {
            
        }

        #endregion
    }
}
