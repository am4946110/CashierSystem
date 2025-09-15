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
    public class SaleDetailsController : Controller
    {
        private readonly CashierSystemContext _context;

        public SaleDetailsController(CashierSystemContext context)
        {
            _context = context;
        }

        // GET: SaleDetails
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext = _context.SaleDetails.Include(s => s.Product).Include(s => s.Sale);
            return View(await cashierSystemContext.ToListAsync());
        }

        // GET: SaleDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleDetail = await _context.SaleDetails
                .Include(s => s.Product)
                .Include(s => s.Sale)
                .FirstOrDefaultAsync(m => m.SaleDetailId == id);
            if (saleDetail == null)
            {
                return NotFound();
            }

            return View(saleDetail);
        }

        // GET: SaleDetails/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleId");
            return View();
        }

        // POST: SaleDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleDetailId,SaleId,ProductId,Quantity,UnitPrice,Total")] SaleDetail saleDetail)
        {
            if (ModelState.IsValid)
            {
                saleDetail.SaleDetailId = Guid.NewGuid();
                _context.Add(saleDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", saleDetail.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // GET: SaleDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleDetail = await _context.SaleDetails.FindAsync(id);
            if (saleDetail == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", saleDetail.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // POST: SaleDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SaleDetailId,SaleId,ProductId,Quantity,UnitPrice,Total")] SaleDetail saleDetail)
        {
            if (id != saleDetail.SaleDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleDetail);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", saleDetail.ProductId);
            ViewData["SaleId"] = new SelectList(_context.Sales, "SaleId", "SaleId", saleDetail.SaleId);
            return View(saleDetail);
        }

        // GET: SaleDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saleDetail = await _context.SaleDetails
                .Include(s => s.Product)
                .Include(s => s.Sale)
                .FirstOrDefaultAsync(m => m.SaleDetailId == id);
            if (saleDetail == null)
            {
                return NotFound();
            }

            return View(saleDetail);
        }

        // POST: SaleDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var saleDetail = await _context.SaleDetails.FindAsync(id);
            if (saleDetail != null)
            {
                _context.SaleDetails.Remove(saleDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleDetailExists(Guid id)
        {
            return _context.SaleDetails.Any(e => e.SaleDetailId == id);
        }
    }
}
