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
    public class PaymentTypesController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public PaymentTypesController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sh = await _context.PaymentTypes.ToListAsync();

            var k = sh.Select(p =>
            {
                p.Id = _keyManager.Protect(p.PaymentTypeId.ToString());

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
                var kk = Guid.Parse(_keyManager.Unprotect(id));

                var paymentType = await _context.PaymentTypes
                    .FirstOrDefaultAsync(m => m.PaymentTypeId == kk);

                if (paymentType == null)
                {
                    return NotFound();
                }

                var sh = await _context.PaymentTypes.ToListAsync();

                var k = sh.Select(p =>
                {
                    p.Id = _keyManager.Protect(p.PaymentTypeId.ToString());

                    return p;

                }).ToList();

                return View(paymentType);
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentTypeId,TypeName")] PaymentType paymentType)
        {
            if (!ModelState.IsValid)
            {
                paymentType.PaymentTypeId = Guid.NewGuid();
                _context.Add(paymentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paymentType);
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

                var paymentType = await _context.PaymentTypes.FindAsync(k);

                if (paymentType == null)
                {
                    return NotFound();
                }

                return View(paymentType);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PaymentTypeId,TypeName")] PaymentType paymentType)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentTypeExists(paymentType.PaymentTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return View(paymentType);
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

                var paymentType = await _context.PaymentTypes
                    .FirstOrDefaultAsync(m => m.PaymentTypeId == k);

                if (paymentType == null)
                {
                    return NotFound();
                }

                return View(paymentType);
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

                var paymentType = await _context.PaymentTypes.FindAsync(k);

                if (paymentType != null)
                {
                    _context.PaymentTypes.Remove(paymentType);
                
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        private bool PaymentTypeExists(Guid id)
        {
            return _context.PaymentTypes.Any(e => e.PaymentTypeId == id);
        }
    }
}
