using CustomerLedger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerLedger.Configure
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure (EntityTypeBuilder<Transaction> builder)
        {
            //Properits 
            //PK-Id
            builder.Property(u => u.Id).HasColumnOrder(0);
            //FK-Id
            builder.Property(u => u.CustomerId).HasColumnOrder(1);
            // Amount
            builder.Property(u => u.Amount).HasColumnOrder(2).IsRequired().HasColumnType("decimal(18,2)");
            // IsPaid
            builder.Property(u => u.IsPaid).HasColumnOrder(3).IsRequired().HasDefaultValue(false);
            //NoteText
            builder.Property(u => u.NoteText).HasColumnOrder(4).IsRequired();
            //Type
            builder.Property(u => u.Type).HasColumnOrder(5).IsRequired();
            //Relationships
            //From CustomerConfiguration
        }
    }
}
