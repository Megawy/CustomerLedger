using CustomerLedger.Data;
using CustomerLedger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerLedger.Controllers
{
    public class LogsController : Controller
    {
        private readonly CustomerLedgerDbContext _context;
        private readonly IConfiguration _configuration;
        public LogsController(CustomerLedgerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;

            var logs = from l in _context.Logs
                       join c in _context.Customers on l.CustomerId equals c.Id
                       select new LogWithCustomer
                       {
                           Log = l,
                           Customer = c
                       };

            // ترتيبات البيانات بناءً على معلمة sortOrder
            switch (sortOrder)
            {
                case "asc":
                    logs = logs.OrderBy(log => log.Log.Timestamp);
                    break;
                case "desc":
                default:
                    logs = logs.OrderByDescending(log => log.Log.Timestamp);
                    break;
            }

            return View(logs.ToList());
        }

    }
}
