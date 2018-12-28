using System;
using System.Collections.Generic;

namespace Remax.Web.Server
{
    /// <summary>
    /// The result of a login request via API
    /// </summary>
    public class UserProfileResultApiModel
    {
        #region Public Properties

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The users username
        /// </summary>
        public string Username { get; set; }

        /// <summary> 
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Users Profile Pic
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Affiliation { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserProfileResultApiModel()
        {
        }

        #endregion
    }


   
}
