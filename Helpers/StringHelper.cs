using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public  static class StringHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NormalizedUpper(this string str)
        {
            return str.ToUpper().Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NormalizedLower(this string str)
        {
            return str.ToLower().Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
