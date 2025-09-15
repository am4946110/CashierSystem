using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Experimental.ProjectCache;

namespace CashierSystem.Controllers
{
    public class VwPaymentsController : Controller
    {
        private readonly CashierSystemContext _context;

        public VwPaymentsController(CashierSystemContext context) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sh = _context.VwPayments.ToList();

            return View(sh);
        }
    }
}
