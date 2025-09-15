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
    public class StockTransactionsController : Controller
    {
        private readonly CashierSystemContext _context;

        public StockTransactionsController(CashierSystemContext context)
        {
            _context = context;
        }

        // GET: StockTransactions
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext = _context.StockTransactions.Include(s => s.Product);
            return View(await cashierSystemContext.ToListAsync());
        }

        // GET: StockTransactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockTransaction = await _context.StockTransactions
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (stockTransaction == null)
            {
                return NotFound();
            }

            return View(stockTransaction);
        }

        // GET: StockTransactions/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return View();
        }

        // POST: StockTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,ProductId,Quantity,TransactionType,TransactionDate,Reference")] StockTransaction stockTransaction)
        {
            if (ModelState.IsValid)
            {
                stockTransaction.TransactionId = Guid.NewGuid();
                _context.Add(stockTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockTransaction.ProductId);
            return View(stockTransaction);
        }

        // GET: StockTransactions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockTransaction = await _context.StockTransactions.FindAsync(id);
            if (stockTransaction == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockTransaction.ProductId);
            return View(stockTransaction);
        }

        // POST: StockTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TransactionId,ProductId,Quantity,TransactionType,TransactionDate,Reference")] StockTransaction stockTransaction)
        {
            if (id != stockTransaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockTransactionExists(stockTransaction.TransactionId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", stockTransaction.ProductId);
            return View(stockTransaction);
        }

        // GET: StockTransactions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockTransaction = await _context.StockTransactions
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (stockTransaction == null)
            {
                return NotFound();
            }

            return View(stockTransaction);
        }

        // POST: StockTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stockTransaction = await _context.StockTransactions.FindAsync(id);
            if (stockTransaction != null)
            {
                _context.StockTransactions.Remove(stockTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockTransactionExists(Guid id)
        {
            return _context.StockTransactions.Any(e => e.TransactionId == id);
        }
    }
}
