using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Remax.Web.Server
{
    /// <summary>
    /// The user data and profile for our application
    /// </summary>
    /// 
    public class ApplicationUser : IdentityUser
    {
        #region Navigational Properties

        /// <summary>
        /// Navigational Property for <see cref="Employee"/>
        /// </summary>
        public Employee Employee { get; set; }
   
        
        #endregion


        #region Default Constructor

        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser() 
        {

        } 
       
        #endregion
    }
}