using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public  static class DateTimeOffsetHelper
    {

        /// <summary>
        /// /
        /// </summary>
        /// <param name="offsetString"></param>
        /// <returns></returns>
        public static DateTimeOffset FromString(string offsetString)
        {

            DateTimeOffset offset;
            if (!DateTimeOffset.TryParse(offsetString, out offset))
                offset = DateTimeOffset.UtcNow;
            return offset;
        }
    }
}
