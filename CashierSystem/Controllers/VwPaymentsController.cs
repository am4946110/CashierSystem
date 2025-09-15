using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.EntityFrameworkCore;

namespace CashierSystem.Controllers
{
    public class VwPaymentsController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public VwPaymentsController(CashierSystemContext context,ICustomKeyManager keyManager) 
        {
            _context = context;
            _keyManager = keyManager;
        }

        public async Task<IActionResult> Index()
        {
            var sh =await _context.VwPayments.ToListAsync();

            var k = sh.Select(p =>
            {
                p.Id = _keyManager.Protect(p.PaymentId.ToString());

                return p;

            }).ToList();

            return View(k);
        }

        public async Task<IActionResult> Details(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var sh = await _context.VwPayments.FirstOrDefaultAsync(s => s.PaymentId == k);

                if (sh == null)
                {
                    return BadRequest();
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
