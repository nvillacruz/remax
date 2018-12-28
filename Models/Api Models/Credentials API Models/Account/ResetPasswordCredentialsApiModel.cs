namespace Remax.Web.Server
{
    /// <summary>
    /// The credentials for  an API client to send Forgot Password link
    /// </summary>
    public class ResetPasswordCredentialsApiModel
    {
        #region Public Properties

        /// <summary>
        /// The users  email
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResetPasswordCredentialsApiModel()
        {
            
        }

        #endregion
    }
}
