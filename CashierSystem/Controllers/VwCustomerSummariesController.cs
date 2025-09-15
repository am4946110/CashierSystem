using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashierSystem.Controllers
{
    public class VwCustomerSummariesController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public VwCustomerSummariesController(CashierSystemContext context,ICustomKeyManager keyManager) 
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sh = _context.VwCustomerSummaries.ToList();

            var k = sh.Select(v =>
            {
                v.Id = _keyManager.Protect(v.CustomerId.ToString());

                return v;
            
            }).ToList();

            return View(k);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var sh = await _context.VwCustomerSummaries.FirstOrDefaultAsync(s => s.CustomerId == k);

                if (sh == null)
                {
                    return NotFound();
                }

                return View(sh);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
