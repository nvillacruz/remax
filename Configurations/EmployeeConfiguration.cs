using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Remax.Web.Server
{
    /// <summary>
    /// Entity Configuration for <see cref="Employee"/>
    /// </summary>
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {

        /// <summary>
        /// Configuration Method
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired().HasMaxLengthAsDefaultEntity();

            builder.Property(x => x.Gender).HasMaxLength(10);
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(64);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(64);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(64);
            builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(16);
            builder.Property(p => p.ImageSource).IsRequired().HasMaxLength(256).HasDefaultValue("users\\");

            builder.Ignore(p => p.FullName);

            //One-to-One relationship
            builder.HasOne(p => p.User).WithOne(x => x.Employee).HasForeignKey<Employee>(y => y.UserId);

            builder.ToTable("Employee");
        }
    }
}
