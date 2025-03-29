namespace CustomerLedger.Models
{
    public class TranWithCst
    {
        public Customer Customer { get; set; }
        public List<Transaction> Transactions { get; set; } // تأكد من أن هذه الخاصية موجودة
        public decimal UnpaidTotal { get; set; }

        // إضافة معلومات إضافية للفاتورة
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
