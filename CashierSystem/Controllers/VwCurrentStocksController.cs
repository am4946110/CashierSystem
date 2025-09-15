using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashierSystem.Controllers
{
    public class VwCurrentStocksController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public VwCurrentStocksController(CashierSystemContext context,ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        public async Task<IActionResult> Index()
        {
            var sh =await _context.VwCurrentStocks.ToListAsync();

            var key = sh.Select(s =>
            {
                s.Id = _keyManager.Protect(s.ProductId.ToString());

                return s;
            
            }).ToList();

            return View(key);
        }

        public async Task<IActionResult> Details(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var keyE = Guid.Parse(_keyManager.Unprotect(id));

                var sh = await  _context.VwCurrentStocks.FirstOrDefaultAsync(s => s.ProductId == keyE);
                if (sh == null)
                {
                    return NotFound();
                }

                return View(sh);
            }
            catch (Exception) 
            {
                return NoContent();
            }
        }
    }
}
