using Microsoft.AspNetCore.Mvc;

namespace CashierSystem.Controllers
{
    public class VwCurrentStocksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
