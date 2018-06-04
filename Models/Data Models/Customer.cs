using System.ComponentModel.DataAnnotations;

namespace Y.Bizz.Web.Server
{
    /// <summary>
    /// Our Settings database table representational model
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The unique Id for this entry
        /// </summary>
        [Key]
        public int Id { get; set; }

        public string Location { get; set; }

        public string Locale { get; set; }

        public string Gender { get; set; }

        #region Navigational Properties


        /// <summary>
        /// Navigational Property for <see cref="ApplicationUser"/>
        /// </summary>
        [Required]
        public ApplicationUser User { get; set; }  // navigation property 

        public string UserId  { get; set; }

        #endregion
    }
}
