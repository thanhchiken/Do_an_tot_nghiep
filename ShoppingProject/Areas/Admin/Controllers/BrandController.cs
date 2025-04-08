using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingProject.Models;
using ShoppingProject.Repository;

[Area("Admin")]
[Route("Admin/Brand")] // Route chính
[Authorize(Roles = "Publisher,Author,Admin")]
public class BrandController : Controller
{
    private readonly DataContext _dataContext;

    public BrandController(DataContext context)
    {
        _dataContext = context;
    }

    [Route("Index")] // Admin/Brand/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await _dataContext.Brands
                .OrderByDescending(p => p.Id)
                .ToListAsync());
    }

    [Route("Create")] // Admin/Brand/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new BrandModel());
    }

    [Route("Create")] // Admin/Brand/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BrandModel brand)
    {
        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name.Replace(" ", "-").ToLower(); // Chuẩn hóa Slug
            var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Thương hiệu đã có trong database");
                return View(brand);
            }

            try
            {
                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra khi lưu dữ liệu!";
                return View(brand);
            }
        }

        TempData["error"] = "Có lỗi xảy ra, vui lòng kiểm tra lại!";
        return View(brand);
    }

    [Route("Edit/{id}")] // Admin/Brand/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var brand = await _dataContext.Brands.FindAsync(id);
        if (brand == null)
        {
            return NotFound();
        }
        return View(brand);
    }

    [Route("Edit/{id}")] // Admin/Brand/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BrandModel brand)
    {
        if (id != brand.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            brand.Slug = brand.Name.Replace(" ", "-").ToLower(); // Chuẩn hóa Slug
            var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brand.Slug && p.Id != brand.Id);
            if (slug != null)
            {
                ModelState.AddModelError("", "Thương hiệu đã có trong database");
                return View(brand);
            }

            try
            {
                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Có lỗi xảy ra khi lưu dữ liệu!";
                return View(brand);
            }
        }

        TempData["error"] = "Có lỗi xảy ra, vui lòng kiểm tra lại!";
        return View(brand);
    }

    //[Route("Delete/{id}")] // Admin/Brand/Delete/{id}
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int Id)
    {
        BrandModel brand = await _dataContext.Brands.FindAsync(Id);

        _dataContext.Brands.Remove(brand);
        await _dataContext.SaveChangesAsync();
        TempData["success"] = "Thương hiệu đã được xóa thành công";
        return RedirectToAction("Index");
    }
}