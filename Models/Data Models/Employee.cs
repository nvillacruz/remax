using Linq.PropertyTranslator.Core;
using System;
using System.Collections.Generic;

namespace Remax.Web.Server
{
    /// <summary>
    /// Our Settings database table representational model
    /// </summary>
    public class Employee
    {
        #region Public Properties

        /// <summary>
        /// The unique Employee Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? BirthDate { get; set; }
        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The users last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ImageSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }

        #endregion

        #region Read-Only Properties

        /// <summary>
        /// 
        /// </summary>
        public string FullName => FullNameExpression.Evaluate(this);

        private static readonly CompiledExpressionMap<Employee, string> FullNameExpression
= DefaultTranslationOf<Employee>.Property(e => e.FullName).Is(e => $"{e.FirstName} {e.LastName}");
        #endregion

        #region Navigational Properties

        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser User { get; set; }


        #endregion
        /// <summary>
        /// 
        /// </summary>
        public Employee()
        {
           
            User = new ApplicationUser();
        }
    }
}
