using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingProject.Repository;

namespace ShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Role")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = context;
            _roleManager = roleManager;
        }
        
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(p => p.Id).ToListAsync());
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Avoid duplicate role
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(model.Name));
            }
            return Redirect("Index");
        }

        [HttpGet]
        [Route("Edit")]

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Handle missing Id
            }
            var role = await _roleManager.FindByIdAsync(id);

            return View(role);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Handle missing Id
            }
            if (ModelState.IsValid) // Validate model state before proceeding
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound(); // Handle role not found
                }
                role.Name = model.Name; // Update role properties with model data
                try
                {
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Role updated successfully!";
                    return RedirectToAction("Index"); // Redirect to the index action
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while updating the role.");
                }

            }
            return View(model ?? new IdentityRole { Id = id });
        }

        //[HttpGet]
        //[Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Handle missing Id
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound(); // Handle role not found
            }

            try
            {
                await _roleManager.DeleteAsync(role);
                TempData["success"] = "Role deleted successfully!";
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while deleting the role.");
            }

            return Redirect("Index");
        }
    }
}
