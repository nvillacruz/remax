using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public  static class SqlDataTypesConfigurationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static PropertyBuilder<decimal?> HasPrecision(this PropertyBuilder<decimal?> builder, int precision, int scale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static PropertyBuilder<decimal> HasPrecision(this PropertyBuilder<decimal> builder, int precision, int scale)
        {
            return builder.HasColumnType($"decimal({precision},{scale})");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PropertyBuilder<decimal> HasPrecisionTwo(this PropertyBuilder<decimal> builder)
        {
            return builder.HasColumnType($"decimal({18},{2})");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PropertyBuilder<string> HasMaxLengthAsEmployee(this PropertyBuilder<string> builder)
        {
            return builder.HasMaxLength(450);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PropertyBuilder<string> HasMaxLengthAsName(this PropertyBuilder<string> builder)
        {
            return builder.HasMaxLength(256);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PropertyBuilder<string> HasMaxLengthAsDefaultEntity(this PropertyBuilder<string> builder)
        {
            return builder.HasMaxLength(64);
        }

    }
}
