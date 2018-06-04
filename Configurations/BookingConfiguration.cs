using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Y.Bizz.Web.Server
{
  public class BookingConfiguration : IEntityTypeConfiguration<Booking>
  {
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
      builder.HasKey(p => p.Id);

      builder.HasOne(p => p.User).WithMany(x => x.Bookings).HasForeignKey(y => y.UserId);

      builder.ToTable("Bookings");
    }
  }
}
