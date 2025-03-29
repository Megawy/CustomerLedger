using CustomerLedger.Data;
using CustomerLedger.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerLedger.Controllers
{
    public class Tran_CstController : Controller
    {
        private readonly CustomerLedgerDbContext _context;
        private readonly IConfiguration _configuration;

        public Tran_CstController(CustomerLedgerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Customer/Customer_Tran
        [HttpGet]
        public IActionResult Index(int id)
        {
            var customer = _context.Customers
                .Include(c => c.Transactions)
                .FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var unpaidTotal = customer.Transactions
                .Where(t => !t.IsPaid)
                .Sum(t => t.Amount);

            var model = new TranWithCst
            {
                Customer = customer,
                Transactions = customer.Transactions.ToList(),
                UnpaidTotal = unpaidTotal,
                InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                InvoiceDate = DateTime.Now,
            };

            return View(model);
        }

        // POST: Customer/GeneratePdf
        [HttpPost]
        public IActionResult GeneratePdf(int id)
        {
            var customer = _context.Customers
        .Include(c => c.Transactions)
        .FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // فلترة المعاملات غير المدفوعة فقط
            var unpaidTransactions = customer.Transactions
                .Where(t => !t.IsPaid)
                .ToList();

            var unpaidTotal = unpaidTransactions
                .Sum(t => t.Amount);

            var model = new TranWithCst
            {
                Customer = customer,
                Transactions = unpaidTransactions, // استخدم المعاملات غير المدفوعة فقط
                UnpaidTotal = unpaidTotal,
                InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                InvoiceDate = DateTime.Now,
            };

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                document.Add(new Paragraph("Invoice")
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Invoice Number: {model.InvoiceNumber}"));
                document.Add(new Paragraph($"Date: {model.InvoiceDate.ToString("yyyyMMddHHmmss")}"));
                document.Add(new Paragraph($"Customer: {model.Customer.Name}"));
                document.Add(new Paragraph($"Discord_Id: {model.Customer.Discordid}"));
                document.Add(new Paragraph("Transactions:")
                    .SetFontSize(16)
                    .SetBold());

                var table = new Table(5);
                table.AddHeaderCell("Date");
                table.AddHeaderCell("Amount");
                table.AddHeaderCell("Is Paid");
                table.AddHeaderCell("Note");
                table.AddHeaderCell("Type");

                foreach (var transaction in model.Transactions)
                {
                    table.AddCell(transaction.Date.ToString("d"));
                    table.AddCell(transaction.Amount.ToString("C"));
                    table.AddCell(transaction.IsPaid ? "Yes" : "No");
                    table.AddCell(transaction.NoteText);
                    table.AddCell(transaction.Type);
                }

                document.Add(table);

                document.Add(new Paragraph($"Total Unpaid Amount: {model.UnpaidTotal.ToString("C")}")
                    .SetFontSize(14)
                    .SetBold());

                document.Close();

                var pdfBytes = stream.ToArray();
                return File(pdfBytes, "application/pdf", $"Invoice_{model.InvoiceNumber}.pdf");
            }
        }
    }
}
