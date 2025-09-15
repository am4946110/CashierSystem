using CashierSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashierSystem.Controllers
{
    public class VwSalesDetailsController : Controller
    {
        private readonly CashierSystemContext _context;
        private readonly ICustomKeyManager _keyManager;

        public VwSalesDetailsController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        // عرض كل المبيعات
        public async Task<IActionResult> Index()
        {
            var sales = await _context.VwSalesDetails.ToListAsync();

            // تشفير الـ SaleId مرة واحدة فقط
            foreach (var sale in sales)
            {
                sale.Id = _keyManager.Protect(sale.SaleId.ToString());
            }

            return View(sales);
        }

        // تفاصيل المبيعات
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            try
            {
                // فك التشفير
                var decryptedId = _keyManager.Unprotect(id);

                // تحويله لـ Guid
                var saleId = Guid.Parse(decryptedId);

                var sale = await _context.VwSalesDetails
                                         .FirstOrDefaultAsync(s => s.SaleId == saleId);

                if (sale == null)
                    return NotFound();

                return View(sale);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
