namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateUserCredentialsApiModel
    {

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// users phone number
        /// </summary>
        /// 
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ImageSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Affiliation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpdateUserCredentialsApiModel()
        {

        }
        #endregion
    }
}
