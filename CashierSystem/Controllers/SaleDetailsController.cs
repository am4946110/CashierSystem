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
    public class SaleDetailsController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public SaleDetailsController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext =await _context.SaleDetails.Include(s => s.Product).Include(s => s.Sale).ToListAsync();

            var k = cashierSystemContext.Select(s =>
            {
                s.Id = _keyManager.Protect(s.SaleDetailId.ToString());

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

                var saleDetail = await _context.SaleDetails
                    .Include(s => s.Product)
                    .Include(s => s.Sale)
                    .FirstOrDefaultAsync(m => m.SaleDetailId == kk);

                if (saleDetail == null)
                {
                    return NotFound();
                }

                var cashierSystemContext = await _context.SaleDetails.Include(s => s.Product).Include(s => s.Sale).ToListAsync();

                var k = cashierSystemContext.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.SaleDetailId.ToString());

                    return s;

                }).ToList();

                return View(saleDetail);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleDetailId,SaleId,ProductId,Quantity,UnitPrice,Total")] SaleDetail saleDetail)
        {
            if (!ModelState.IsValid)
            {
                saleDetail.SaleDetailId = Guid.NewGuid();
                
                _context.Add(saleDetail);
                
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", saleDetail.ProductId);
            
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", saleDetail.SaleId);
            
            return View(saleDetail);
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

                var saleDetail = await _context.SaleDetails.FindAsync(k);

                if (saleDetail == null)
                {
                    return NotFound();
                }

                ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", saleDetail.ProductId);

                ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", saleDetail.SaleId);

                return View(saleDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SaleDetailId,SaleId,ProductId,Quantity,UnitPrice,Total")] SaleDetail saleDetail)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleDetailExists(saleDetail.SaleDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", saleDetail.ProductId);

            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleDate", saleDetail.SaleId);
            
            return View(saleDetail);
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

                var saleDetail = await _context.SaleDetails
                    .Include(s => s.Product)
                    .Include(s => s.Sale)
                    .FirstOrDefaultAsync(m => m.SaleDetailId == k);

                if (saleDetail == null)
                {
                    return NotFound();
                }

                return View(saleDetail);
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var k = Guid.Parse(_keyManager.Unprotect(id));

                var saleDetail = await _context.SaleDetails.FindAsync(k);

                if (saleDetail != null)
                {
                    _context.SaleDetails.Remove(saleDetail);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        private bool SaleDetailExists(Guid id)
        {
            return _context.SaleDetails.Any(e => e.SaleDetailId == id);
        }
    }
}
