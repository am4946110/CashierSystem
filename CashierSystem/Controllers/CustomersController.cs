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
    public class CustomersController : Controller
    {
        private readonly CashierSystemContext _context;
        
        private readonly ICustomKeyManager _keyManager;

        public CustomersController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sh = await _context.Customers.ToListAsync();

            var ke = sh.Select(s =>
            {
                s.Id = _keyManager.Protect(s.CustomerId.ToString());

                return s;
            
            }).ToList();

            return View(ke);
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
                var ke = Guid.Parse(_keyManager.Unprotect(id));

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == ke);

                if (customer == null)
                {
                    return NotFound();
                }

                var sh = await _context.Customers.ToListAsync();

                var kee = sh.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.CustomerId.ToString());

                    return s;

                }).ToList();

                return View(customer);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Phone,Email,Address,DateBecameCustomer")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                customer.CustomerId = Guid.NewGuid();
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
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

                var customer = await _context.Customers.FindAsync(k);

                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CustomerId,FullName,Phone,Email,Address,DateBecameCustomer")] Customer customer)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(customer);
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

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerId == k);

                if (customer == null)
                {
                    return NotFound();
                }

                return View(customer);
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

                var customer = await _context.Customers.FindAsync(k);

                if (customer != null)
                {
                    _context.Customers.Remove(customer);

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
