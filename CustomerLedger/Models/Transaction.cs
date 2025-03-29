namespace CustomerLedger.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsPaid { get; set; }
        public string NoteText { get; set; }
        public string Type { get; set; }
        public Customer Customer { get; set; }
    }
}
