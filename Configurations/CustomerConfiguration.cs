using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Y.Bizz.Web.Server
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.User).WithOne(x => x.Customer).HasForeignKey<Customer>(y=> y.UserId) ;

            builder.ToTable("Customers");
        }
    }
}
