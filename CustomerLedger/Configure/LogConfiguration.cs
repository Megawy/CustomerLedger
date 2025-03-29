using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CustomerLedger.Models;

namespace CustomerLedger.Configure
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {

        public void Configure(EntityTypeBuilder<Log> builder)
        {
            //Properits 
            //PK-Id
            builder.Property(u => u.Id).HasColumnOrder(0);
            //FK-Id
            builder.Property(u => u.CustomerId).HasColumnOrder(1);
            // Action
            builder.Property(u => u.Action).HasColumnOrder(2).IsRequired();
            //Details
            builder.Property(u => u.Details).HasColumnOrder(3);
            //Relationships
            //From CustomerConfiguration
        }
    }
}
