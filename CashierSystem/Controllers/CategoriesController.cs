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
    public class CategoriesController : Controller
    {
        private readonly CashierSystemContext _context;

        private readonly ICustomKeyManager _keyManager;

        public CategoriesController(CashierSystemContext context, ICustomKeyManager keyManager)
        {
            _context = context;
            _keyManager = keyManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sh = await _context.Categories.ToListAsync();

            var ke = sh.Select(s =>
            {
                s.Id = _keyManager.Protect(s.CategoryId.ToString());

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

                var category = await _context.Categories
                    .FirstOrDefaultAsync(m => m.CategoryId == ke);
                
                if (category == null)
                {
                    return NotFound();
                }

                var sh = await _context.Categories.ToListAsync();

                var kee = sh.Select(s =>
                {
                    s.Id = _keyManager.Protect(s.CategoryId.ToString());

                    return s;

                }).ToList();


                return View(category);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description")] Category category)
        {
            if (!ModelState.IsValid)
            {
                category.CategoryId = Guid.NewGuid();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
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

                var kee = Guid.Parse(_keyManager.Unprotect(id));

                var category = await _context.Categories.FindAsync(kee);

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryId,CategoryName,Description")] Category category)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(category);
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
                var kee = Guid.Parse(_keyManager.Unprotect(id));

                var category = await _context.Categories
                    .FirstOrDefaultAsync(m => m.CategoryId == kee);

                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
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
                var ke = Guid.Parse(_keyManager.Unprotect(id));

                var category = await _context.Categories.FindAsync(ke);

                if (category != null)
                {
                    _context.Categories.Remove(category);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception) 
            {
                return BadRequest();
            }
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
