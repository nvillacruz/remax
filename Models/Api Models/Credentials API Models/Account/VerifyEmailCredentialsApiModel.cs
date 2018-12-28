namespace Remax.Web.Server
{
    /// <summary>
    /// The credentials for  an API client to send Forgot Password link
    /// </summary>
    public class VerifyEmailCredentialsApiModel
    {
        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EmailToken { get; set; }



        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public VerifyEmailCredentialsApiModel()
        {
            
        }

        #endregion
    }
}
