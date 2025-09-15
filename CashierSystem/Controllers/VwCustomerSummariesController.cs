using Microsoft.AspNetCore.Mvc;

namespace CashierSystem.Controllers
{
    public class VwCustomerSummariesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
