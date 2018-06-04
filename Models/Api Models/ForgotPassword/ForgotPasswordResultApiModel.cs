namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// The result of a login request via API
    /// </summary>
    public class ForgotPasswordResultApiModel
    {
        #region Public Properties

        public string CallBackUrl { get; set; }

     
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
