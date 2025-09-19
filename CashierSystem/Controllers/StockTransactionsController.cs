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

        private readonly ICustomKeyManager _keyManager;

        public StockTransactionsController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cashierSystemContext = await _context.StockTransactions.Include(s => s.Product).ToListAsync();

            var k = cashierSystemContext.Select(s =>
            {
                s.Id = _keyManager.Protect(s.TransactionId.ToString());

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

                var stockTransaction = await _context.StockTransactions
                    .Include(s => s.Product)
                    .FirstOrDefaultAsync(m => m.TransactionId == kk);

                if (stockTransaction == null)
                {
                    return NotFound();
                }

                var cashierSystemContext = await _context.StockTransactions.Include(s => s.Product).ToListAsync();

                var k = cashierSystemContext.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.TransactionId.ToString());

                    return s;

                }).ToList();

                return View(stockTransaction);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Quantity,TransactionType,TransactionDate,Reference")] StockTransaction stockTransaction)
        {
            if (!ModelState.IsValid)
            {
                stockTransaction.TransactionId = Guid.NewGuid();
                _context.Add(stockTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stockTransaction.ProductId);
            return View(stockTransaction);
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

                var stockTransaction = await _context.StockTransactions.FindAsync(k);

                if (stockTransaction == null)
                {
                    return NotFound();
                }

                ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stockTransaction.ProductId);

                return View(stockTransaction);
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TransactionId,ProductId,Quantity,TransactionType,TransactionDate,Reference")] StockTransaction stockTransaction)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockTransaction);
                    
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
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
            }
            
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", stockTransaction.ProductId);
            
            return View(stockTransaction);
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

                var stockTransaction = await _context.StockTransactions
                    .Include(s => s.Product)
                    .FirstOrDefaultAsync(m => m.TransactionId == k);

                if (stockTransaction == null)
                {
                    return NotFound();
                }

                return View(stockTransaction);
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

                var stockTransaction = await _context.StockTransactions.FindAsync(k);

                if (stockTransaction != null)
                {
                    _context.StockTransactions.Remove(stockTransaction);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        private bool StockTransactionExists(Guid id)
        {
            return _context.StockTransactions.Any(e => e.TransactionId == id);
        }
    }
}
