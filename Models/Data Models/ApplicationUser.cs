using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// The user data and profile for our application
    /// </summary>
    public class ApplicationUser : IdentityUser
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
        /// Navigational property for <see cref="Booking"/>
        /// </summary>
        public ICollection<Booking> Bookings { get; set; }


        /// <summary>
        /// Navigational Property for <see cref="Customer"/>
        /// </summary>
        public Customer Customer { get; set; }

        #endregion

        public ApplicationUser()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}