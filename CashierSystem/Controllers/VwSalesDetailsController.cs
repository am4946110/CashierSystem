using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CashierSystem.Controllers
{
    public class VwSalesDetailsController : Controller
    {
        private readonly CashierSystemContext _context;

        public VwSalesDetailsController(CashierSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sh = _context.VwSalesDetails.ToList();

            return View(sh);
        }
    }
}
