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
    public class SuppliersController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public SuppliersController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sh = await _context.Suppliers.ToListAsync();

            var k = sh.Select(s =>
            {
                s.Id = _keyManager.Protect(s.SupplierId.ToString());

                return s;
            
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

                var supplier = await _context.Suppliers
                    .FirstOrDefaultAsync(m => m.SupplierId == kk);

                if (supplier == null)
                {
                    return NotFound();
                }

                var sh = await _context.Suppliers.ToListAsync();

                var k = sh.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.SupplierId.ToString());

                    return s;

                }).ToList();

                return View(supplier);
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
        public async Task<IActionResult> Create([Bind("SupplierId,SupplierName,Phone,Email,Address")] Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                supplier.SupplierId = Guid.NewGuid();
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
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

                var supplier = await _context.Suppliers.FindAsync(k);

                if (supplier == null)
                {
                    return NotFound();
                }

                return View(supplier);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SupplierId,SupplierName,Phone,Email,Address")] Supplier supplier)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    
                    await _context.SaveChangesAsync();
                   
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.SupplierId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            return View(supplier);
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

                var supplier = await _context.Suppliers
                    .FirstOrDefaultAsync(m => m.SupplierId == k);

                if (supplier == null)
                {
                    return NotFound();
                }

                return View(supplier);
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

                var supplier = await _context.Suppliers.FindAsync(id);

                if (supplier != null)
                {
                    _context.Suppliers.Remove(supplier);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        private bool SupplierExists(Guid id)
        {
            return _context.Suppliers.Any(e => e.SupplierId == id);
        }
    }
}
