using CustomerLedger.Configure;
using CustomerLedger.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerLedger.Data
{
    public class CustomerLedgerDbContext : DbContext
    {

        public CustomerLedgerDbContext(DbContextOptions<CustomerLedgerDbContext> options)
         : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customer Configuration
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            // Logs Configuration
            modelBuilder.ApplyConfiguration(new LogConfiguration());
            // Transaction Configuration
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        }
    }
}
