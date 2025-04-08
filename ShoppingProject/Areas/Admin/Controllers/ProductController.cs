using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingProject.Models;
using ShoppingProject.Repository;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    [Authorize]
    //[Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Admin/Product/Index (Danh sách sản phẩm)
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var products = await _dataContext.Products
                .OrderByDescending(p => p.Id)
                .Include(c => c.Category)
                .Include(b => b.Brand)
                .ToListAsync();
            return View(products);
        }

        // GET: /Admin/Product/Create (Trang tạo sản phẩm)
        [HttpGet]
        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        // POST: /Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryID);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandID);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-").ToLower();
                var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("", "Sản phẩm với tên này đã tồn tại trong database.");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
                    Directory.CreateDirectory(uploadsDir); // Đảm bảo thư mục tồn tại
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    product.Image = imageName;
                }
                else
                {
                    product.Image = "noname.jpg"; // Giá trị mặc định nếu không có ảnh
                }

                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Có lỗi trong dữ liệu nhập vào: " + string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(product);
        }

        // GET: /Admin/Product/Edit/{id}
        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryID);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandID);
            return View(product);
        }

        // POST: /Admin/Product/Edit/{id}
        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID không khớp");
            }

            var existedProduct = await _dataContext.Products.FindAsync(id);
            if (existedProduct == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryID);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandID);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-").ToLower();
                var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug && p.Id != product.Id);
                if (existingProduct != null)
                {
                    ModelState.AddModelError("", "Sản phẩm với tên này đã tồn tại cho sản phẩm khác.");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
                    Directory.CreateDirectory(uploadsDir);
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    string oldFilePath = Path.Combine(uploadsDir, existedProduct.Image);

                    if (System.IO.File.Exists(oldFilePath) && existedProduct.Image != "noname.jpg")
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    existedProduct.Image = imageName;
                }

                existedProduct.Name = product.Name;
                existedProduct.Description = product.Description;
                existedProduct.Price = product.Price;
                existedProduct.CategoryID = product.CategoryID;
                existedProduct.BrandID = product.BrandID;
                existedProduct.Slug = product.Slug;

                _dataContext.Update(existedProduct);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Có lỗi trong dữ liệu nhập vào: " + string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (!string.Equals(product.Image, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
                string oldfilePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "sản phẩm đã được xóa thành công";
            return RedirectToAction("Index");
        }

        
    }
}