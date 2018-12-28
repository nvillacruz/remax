namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateAccountSettingCredentialsApiModel
    {

        #region Public Properties

        /// <summary>
        /// The users password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OldPassword { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateAccountSettingCredentialsApiModel()
        {

        }
        #endregion
    }
}