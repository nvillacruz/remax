namespace Remax.Web.Server
{
    /// <summary>
    /// The result of a login request via API
    /// </summary>
    public class ForgotPasswordResultApiModel
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public string CallBackUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ForgotPasswordResultApiModel()
        {
            
        }

        #endregion
    }
}
