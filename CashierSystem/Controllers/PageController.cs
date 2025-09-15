using Microsoft.AspNetCore.Mvc;

namespace CashierSystem.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Homrshow()
        {
            return PartialView();
        }
    }
}
