using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using CustomerLedger.Models;

namespace CustomerLedger.Configure
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            //Properits 
            //Id
            builder.Property(u => u.Id).HasColumnOrder(0);
            //Name
            builder.HasIndex(u => u.Name);
            builder.Property(u => u.Name).HasMaxLength(200).IsRequired().HasColumnOrder(1);
            //Email
            builder.HasIndex(u => u.Email);
            builder.Property(u => u.Email).HasMaxLength(100).HasColumnOrder(2);
            //Discord_ID 
            builder.HasIndex(u => u.Discordid);
            builder.Property(u => u.Discordid).HasMaxLength(250).HasColumnOrder(4).IsRequired();
            //phone 
            builder.HasIndex(u => u.Phone);
            builder.Property(u => u.Phone).HasMaxLength(22).HasColumnOrder(3);

            //Relationships
            // Customer - Logs Relationship (One-to-Many)
            builder
                 .HasMany(u => u.Logs)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);
            // Customer - Tran Relationship (One-to-Many)
            builder
                .HasMany(u => u.Transactions)
               .WithOne(a => a.Customer)
               .HasForeignKey(a => a.CustomerId);
        }

    }
}
