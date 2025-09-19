using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using static CashierSystem.Models.ViewModels;
using CashierSystem.Models;

namespace TMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesController : Controller
    {
        private readonly RoleManager<MyRole> _roleManager;
        private readonly ICustomKeyManager _keyManager;


        public ManageRolesController(ICustomKeyManager keyManager, RoleManager<MyRole> roleManager)
        {
            _roleManager = roleManager;
            _keyManager = keyManager;
        }


        public IActionResult Index()
        {
            var roles = _roleManager.Roles
                .Select(r => new MyRole
                {
                    EncryptedId = _keyManager.Protect(r.Id.ToString()),
                    Name = r.Name,
                    NormalizedName = r.NormalizedName
                    
                })
                .ToList();

            return View(roles);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("", "Role name is required.");
                return View();
            }

            try
            {

                var existingRole = await _roleManager.FindByNameAsync(name);
                if (existingRole != null)
                {
                    ModelState.AddModelError("", "The role already exists.");
                    return View();
                }

                var role = new MyRole { Name = name };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }

            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Role ID is required.");
            }

            string roleId;
            try
            {
                roleId = _keyManager.Unprotect(id);
            }
            catch (Exception)
            {
                return BadRequest("Invalid role ID.");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Id))
            {
                return BadRequest("Role ID is required.");
            }

            var existingRole = await _roleManager.FindByIdAsync(role.Id);
            if (existingRole == null)
            {
                return NotFound();
            }

            existingRole.Name = role.Name;

            var result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Error: {error.Description}");
                ModelState.AddModelError("", error.Description);
            }

            return View(role);
        }

    }
}