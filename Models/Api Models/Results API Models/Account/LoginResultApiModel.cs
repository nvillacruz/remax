using System;
using System.Collections.Generic;

namespace Remax.Web.Server
{
    /// <summary>
    /// The result of a login request via API
    /// </summary>
    public class LoginResultApiModel
    {
        #region Public Properties

        /// <summary>
        /// The authentication token used to stay authenticated through future requests
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Users Profile Pic
        /// </summary>
        public string ImageSource { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoginResultApiModel()
        {
            //Memberships = new List<MembershipResultApiModel>();
        }

        #endregion
    }


   
}
