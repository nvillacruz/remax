using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Remax.Web.Server
{
    /// <summary>
    /// Extension methods for aggregating all types of errors
    /// </summary>
    public static class AggregateErrorsExtensions
    {

        /// <summary>
        /// Combines all errors into a single string
        /// </summary>
        /// <param name="errors">The errors to aggregate</param>
        /// <returns>Returns a string with each error separated by a new line</returns>
        public static string AggregateErrors(this IEnumerable<IdentityError> errors)
        {
            // Get all errors into a list
            return errors?.ToList()
                          // Grab their description
                          .Select(f => f.Description)
                          // And combine them with a newline separator
                          .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
        }



        /// <summary>
        /// Combines all errors into a single string
        /// </summary>
        /// <param name="errors">The errors to aggregate</param>
        /// <returns>Returns a string with each error separated by a new line</returns>
        public static string AggregateErrors(this ModelStateDictionary errors)
        {
            // Get all errors into a list
            return errors?.Values
                          // Grab their description
                          .SelectMany(f => f.Errors)
                          .Select(x=> x.ErrorMessage)
                          // And combine them with a newline separator
                          .Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");
        }


    }
}
