using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingProject.Models;
using ShoppingProject.Models.ViewModels;
using ShoppingProject.Repository;

namespace ShoppingProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            var products = _dataContext.Products.ToList(); // Lấy danh sách sản phẩm
            return View(products);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _dataContext.Products
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .ToListAsync();

            ViewBag.Keyword = searchTerm;

            return View(products);
        }
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var productsById = await _dataContext.Products.Include(p=> p.Ratings)
                .Where(p => p.Id == Id).FirstOrDefaultAsync();
            //related Product
            var relatedProducts = await _dataContext.Products
            .Where(p => p.CategoryID == productsById.CategoryID && p.Id != productsById.Id)
            .Take(4)
            .ToListAsync();
            ViewBag.RelatedProducts = relatedProducts;

            var viewModel = new ProductDetailsViewModel
            {
                ProductDetails = productsById,
                
            };
            return View(viewModel);
        }
        public async Task<IActionResult> CommentProduct(RatingModel rating)
        {
            if (ModelState.IsValid)
            {

                var ratingEntity = new RatingModel
                {
                    ProductId = rating.ProductId,
                    Name = rating.Name,
                    Email = rating.Email,
                    Comment = rating.Comment,
                    Start = rating.Start

                };

                _dataContext.Ratings.Add(ratingEntity);
                await _dataContext.SaveChangesAsync();

                TempData["success"] = "Thêm đánh giá thành công";

                return Redirect(Request.Headers["Referer"]);
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);

                return RedirectToAction("Detail", new { id = rating.ProductId });
            }

            return Redirect(Request.Headers["Referer"]);
        }
    }
}