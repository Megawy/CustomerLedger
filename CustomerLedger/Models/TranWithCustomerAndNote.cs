namespace CustomerLedger.Models
{
    public class TranWithCustomerAndNote
    {
        public Customer Customer { get; set; }
        public Transaction Transaction { get; set; }
        public Log Log { get; set; }

    }
}
