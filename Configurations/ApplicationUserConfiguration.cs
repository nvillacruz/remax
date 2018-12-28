using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Remax.Web.Server
{
    /// <summary>
    /// Entity Configuration for <see cref="ApplicationUser"/>
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// Configuration Method
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           
            //builder.Property(p => p.Id).HasMaxLengthAsDefaultEntity();
        }
    }
}
