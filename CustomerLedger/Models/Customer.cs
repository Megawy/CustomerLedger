namespace CustomerLedger.Models
{
    public class Customer
    {
        //Record
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string Discordid { get; set; }

        //Relationships
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}
