using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseDataModel
    {
        #region Public Properties


        /// <summary>
        /// The Date when the entry is created
        /// </summary>
        //[Column(Order =100)]
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The date when the entry is last modified
        /// </summary>
        //[Column(Order = 101)]
        public DateTimeOffset DateModified { get; set; }


        /// <summary>
        /// User Id who created the entry
        /// </summary>
        //[Column(Order = 102)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// User Id who last modified the entry
        /// </summary>
        //[Column(Order = 103)]
        public string ModifiedBy  { get; set; }


        //public ApplicationUser UserCreated { get; set; }

        //public ApplicationUser UserModified { get; set; }
        #endregion
    }
}