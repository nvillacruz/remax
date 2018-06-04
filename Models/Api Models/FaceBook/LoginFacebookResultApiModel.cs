namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// The result of a facebook login request via API
    /// </summaryLoginFacebookResultApiModel
    public class LoginFacebookResultApiModel
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
        public LoginFacebookResultApiModel()
        {
            
        }

        #endregion
    }
}
