using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CashierSystem.Models;

namespace CashierSystem.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public PaymentsController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext =await _context.Payments.Include(p => p.PaymentType).Include(p => p.Sale).ToListAsync();
            
            var k = cashierSystemContext.Select(p =>
            {

                p.Id = _keyManager.Protect(p.PaymentId.ToString());

                return p;

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

                var payment = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Include(p => p.Sale)
                    .FirstOrDefaultAsync(m => m.PaymentId == k);

                if (payment == null)
                {
                    return NotFound();
                }

                var cashierSystemContext = await _context.Payments.Include(p => p.PaymentType).Include(p => p.Sale).ToListAsync();

                var kh = cashierSystemContext.Select(p =>
                {

                    p.Id = _keyManager.Protect(p.PaymentId.ToString());

                    return p;

                }).ToList();

                return View(payment);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName");
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,SaleId,PaymentTypeId,Amount,PaidAt")] Payment payment)
        {
            if (!ModelState.IsValid)
            {
                payment.PaymentId = Guid.NewGuid();
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName", payment.PaymentTypeId);
            
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", payment.SaleId);
            
            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var payment = await _context.Payments.FindAsync(k);

                if (payment == null)
                {
                    return NotFound();
                }

                ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName", payment.PaymentTypeId);

                ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", payment.SaleId);

                return View(payment);
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PaymentId,SaleId,PaymentTypeId,Amount,PaidAt")] Payment payment)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "PaymentTypeId", "TypeName", payment.PaymentTypeId);
            
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", payment.SaleId);
            
            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var payment = await _context.Payments
                    .Include(p => p.PaymentType)
                    .Include(p => p.Sale)
                    .FirstOrDefaultAsync(m => m.PaymentId == k);

                if (payment == null)
                {
                    return NotFound();
                }

                return View(payment);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var payment = await _context.Payments.FindAsync(k);

                if (payment != null)
                {
                    _context.Payments.Remove(payment);
                
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        private bool PaymentExists(Guid id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
