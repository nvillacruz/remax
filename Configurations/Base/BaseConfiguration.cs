using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// Entity Configuration for <see cref="BaseDataModel"/>
    /// </summary>
    public class BaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseDataModel
    {
        /// <summary>
        /// Configuration Method
        /// </summary>
        /// <param name="builder"></param>
         public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.CreatedBy).IsRequired().HasMaxLengthAsEmployee();
            builder.Property(p => p.ModifiedBy).IsRequired().HasMaxLengthAsEmployee();
            builder.Property(p => p.DateCreated).IsRequired().HasDefaultValue(DateTimeOffset.UnixEpoch);
            builder.Property(p => p.DateModified).IsRequired().HasDefaultValue(DateTimeOffset.UnixEpoch);
        }

       
    }
}
