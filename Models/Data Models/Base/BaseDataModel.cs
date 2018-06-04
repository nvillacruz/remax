using System;

namespace Y.Bizz.Web.Server
{
    public class BaseDataModel
    {
        #region Public Properties


        /// <summary>
        /// The Date when the entry is created
        /// </summary>
        public DateTimeOffset DateCreated { get; set; }

        /// <summary>
        /// The date when the entry is last modified
        /// </summary>
        public DateTimeOffset DateModified { get; set; }
        
        /// <summary>
        /// User Id who created the entry
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User Id who last modified the entry
        /// </summary>
        public string Modifiedby  { get; set; }


        public ApplicationUser UserCreated { get; set; }

        public ApplicationUser UserModified { get; set; }
        #endregion
    }
}