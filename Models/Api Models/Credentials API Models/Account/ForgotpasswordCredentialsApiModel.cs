namespace Remax.Web.Server
{
    /// <summary>
    /// The credentials for  an API client to send Forgot Password link
    /// </summary>
    public class ForgotPasswordCredentialsApiModel
    {
        #region Public Properties

        /// <summary>
        /// The users  email
        /// </summary>
        public string Email { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ForgotPasswordCredentialsApiModel()
        {
            
        }

        #endregion
    }
}
