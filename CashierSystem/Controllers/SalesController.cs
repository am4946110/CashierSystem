using CashierSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ApplicationDbContext _application;

        private readonly ICustomKeyManager _keyManager;

        public SalesController(CashierSystemContext context, ApplicationDbContext application,ICustomKeyManager keyManager)
        {
            _context = context;
            _application = application;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext = await _context.Sales.Include(s => s.Customer).ToListAsync();

            var k = cashierSystemContext.Select(s =>
            {
                s.Id = _keyManager.Protect(s.SaleId.ToString());

                return s;

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

                var kk = Guid.Parse(_keyManager.Unprotect(id));

                var sale = await _context.Sales
                    .Include(s => s.Customer)
                    .FirstOrDefaultAsync(m => m.SaleId == kk);

                if (sale == null)
                {
                    return NotFound();
                }

                var cashierSystemContext = await _context.Sales.Include(s => s.Customer).ToListAsync();

                var k = cashierSystemContext.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.SaleId.ToString());

                    return s;

                }).ToList();

                return View(sale);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Message"] = new SelectList(_application.MyUsers, "Id", "UserName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,CustomerId,UserId,SaleDate,TotalAmount,Discount,NetAmount")] Sale sale)
        {
            if (!ModelState.IsValid)
            {
                sale.SaleId = Guid.NewGuid();
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Message"] = new SelectList(_application.MyUsers, "Id", "UserName",sale.UserId);
            
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", sale.CustomerId);
            
            return View(sale);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var sale = await _context.Sales.FindAsync(k);

                if (sale == null)
                {
                    return NotFound();
                }
                ViewData["Message"] = new SelectList(_application.MyUsers, "Id", "UserName", sale.UserId);
                ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", sale.CustomerId);
                return View(sale);
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SaleId,CustomerId,UserId,SaleDate,TotalAmount,Discount,NetAmount")] Sale sale)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.SaleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewData["Message"] = new SelectList(_application.MyUsers, "Id", "UserName", sale.UserId);
            
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", sale.CustomerId);
            
            return View(sale);
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

                var sale = await _context.Sales
                    .Include(s => s.Customer)
                    .FirstOrDefaultAsync(m => m.SaleId == k);

                if (sale == null)
                {
                    return NotFound();
                }

                return View(sale);
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

                var sale = await _context.Sales.FindAsync(k);

                if (sale != null)
                {
                    _context.Sales.Remove(sale);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        private bool SaleExists(Guid id)
        {
            return _context.Sales.Any(e => e.SaleId == id);
        }
    }
}
