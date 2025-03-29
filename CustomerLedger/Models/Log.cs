namespace CustomerLedger.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Action { get; set; } 
        public string Details { get; set; } 
        public DateTime Timestamp { get; set; } 
        public int CustomerId { get; set; } 
        public Customer Customer { get; set; }
    }
}
