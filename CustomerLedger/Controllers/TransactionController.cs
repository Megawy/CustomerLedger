using CustomerLedger.Data;
using CustomerLedger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace CustomerLedger.Controllers
{
    public class TransactionController : Controller
    {
        private readonly CustomerLedgerDbContext _context;
        private readonly IConfiguration _configuration;
        public TransactionController(CustomerLedgerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        //Transaction 
        //Transaction Get
        [HttpGet]
        public IActionResult Index()
        {
            var transactions = _context.Transactions
        .Include(t => t.Customer)
        .Select(t => new TranWithCustomerAndNote
        {
            Customer = t.Customer,
            Transaction = t,
        }).ToList();

            return View(transactions);
        }
        //Transaction Add
        //Get Add 
        [HttpGet]
        public IActionResult Add()
        {
            // Populate the dropdown list for customers
            ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");

            // Return the view without a model
            return View();
        }
        //Post Add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TranWithCustomerAndNote model)
        {

            // التحقق من وجود العميل قبل إضافة المعاملة
            var customerExists = _context.Customers.Any(c => c.Id == model.Transaction.CustomerId);
            if (!customerExists)
            {
                ModelState.AddModelError("Transaction.CustomerId", "Invalid Customer Id.");
                ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");
                return View(model);
            }

            var transaction = model.Transaction;
            transaction.Date = DateTime.Now;
            _context.Transactions.Add(transaction);
            var result = _context.SaveChanges();

            if (result > 0)
            {
                var log = new Log
                {
                    Action = "Transaction Added",
                    CustomerId = transaction.CustomerId,
                    Timestamp = DateTime.Now,
                    Details = $"Transaction Added: Amount = {transaction.Amount}, IsPaid = {transaction.IsPaid}, Date = {transaction.Date}, Note = {transaction.NoteText}, Type = {transaction.Type}"
                };

                _context.Logs.Add(log);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Customers = new SelectList(_context.Customers, "Id", "Name");
                return View(model);
            }
        }
        //Transaction Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the transaction with the specified ID
            var transaction = _context.Transactions
                .Include(t => t.Customer) // Include related customer information if needed
                .FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound(); // Return a 404 error if the transaction is not found
            }

            // Create a model object that includes transaction and customer information
            var model = new TranWithCustomerAndNote
            {
                Transaction = transaction,
                Customer = transaction.Customer,
                // Add any additional properties as needed
            };

            return View(model); // Pass the model to the view
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TranWithCustomerAndNote model)
        {
            var transaction = _context.Transactions
                .Include(t => t.Customer) // Include related customer information
                .FirstOrDefault(t => t.Id == model.Transaction.Id);

            if (transaction == null)
            {
                return NotFound(); // Return a 404 error if the transaction is not found
            }

            // Store original values for logging purposes
            var originalTransaction = new
            {
                Amount = transaction.Amount,
                IsPaid = transaction.IsPaid,
                Date = transaction.Date,
                NoteText = transaction.NoteText,
                Type = transaction.Type
            };

            // Update transaction properties
            transaction.Amount = model.Transaction.Amount;
            transaction.IsPaid = model.Transaction.IsPaid;
            transaction.Date = model.Transaction.Date;
            transaction.NoteText = model.Transaction.NoteText;
            transaction.Type = model.Transaction.Type;

            // Mark the entity as modified
            _context.Update(transaction);
            var result = _context.SaveChanges(); // Save changes to the database
            if (result > 0)
            {
                // Log the changes made
                var changes = new List<string>();

                if (originalTransaction.Amount != transaction.Amount)
                    changes.Add($"Amount changed from {originalTransaction.Amount} to {transaction.Amount}");

                if (originalTransaction.IsPaid != transaction.IsPaid)
                    changes.Add($"IsPaid changed from {originalTransaction.IsPaid} to {transaction.IsPaid}");

                if (originalTransaction.Date != transaction.Date)
                    changes.Add($"Date changed from {originalTransaction.Date} to {transaction.Date}");

                if (originalTransaction.NoteText != transaction.NoteText)
                    changes.Add($"NoteText changed from '{originalTransaction.NoteText}' to '{transaction.NoteText}'");

                if (originalTransaction.Type != transaction.Type)
                    changes.Add($"Type changed from '{originalTransaction.Type}' to '{transaction.Type}'");

                var log = new Log
                {
                    Action = "Customer Edit Transaction",
                    CustomerId = transaction.Customer.Id, // Use the customer ID
                    Timestamp = DateTime.Now,
                    Details = $"Customer Edit Info: {string.Join(", ", changes)}",
                };

                // Add the log entry to the context and save
                _context.Logs.Add(log);
                _context.SaveChanges();

                return RedirectToAction("Index"); // Redirect to a different action or view
            }
            // If saving changes fails, return the view with the current model
            ModelState.AddModelError("", "Unable to save changes. Please try again.");
            return View(model);
        }
        //Transaction Del
        // GET: Transaction/Delete/
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the transaction with the specified ID
            var transaction = _context.Transactions
                .Include(t => t.Customer) // Include related customer information if needed
                .FirstOrDefault(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound(); // Return a 404 error if the transaction is not found
            }

            // Create a model object that includes transaction and customer information
            var model = new TranWithCustomerAndNote
            {
                Transaction = transaction,
                Customer = transaction.Customer,
                // Add any additional properties as needed
            };

            return View(model); // Pass the model to the view
        }
        // POST: Transaction/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TranWithCustomerAndNote model)
        {
            var transaction = _context.Transactions
                .Include(t => t.Customer) // Include related customer information
                .FirstOrDefault(t => t.Id == model.Transaction.Id);

            if (transaction == null)
            {
                return NotFound(); // Return a 404 error if the transaction is not found
            }

            // Store original values for logging purposes
            var log = new Log
            {
                Action = "Customer Delete Transaction",
                CustomerId = transaction.Customer.Id, // Use the customer ID
                Timestamp = DateTime.Now,
                Details = $"Transaction Deleted: Amount = {transaction.Amount}, IsPaid = {transaction.IsPaid}, Date = {transaction.Date}, Note = {transaction.NoteText}, Type = {transaction.Type}"
            };

            // Remove the transaction
            _context.Transactions.Remove(transaction);

            // Save changes to the database
            var result = _context.SaveChanges();

            if (result > 0)
            {
                // Add the log entry to the context and save
                _context.Logs.Add(log);
                _context.SaveChanges();

                return RedirectToAction("Index"); // Redirect to a different action or view
            }

            // If saving changes fails, return the view with the current model
            ModelState.AddModelError("", "Unable to delete the transaction. Please try again.");
            return View(model);
        }
    }
}

