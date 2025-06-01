using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingProject.Models;
using ShoppingProject.Repository;
using System.Globalization;

namespace ShoppingProject.Controllers
{
    public class BrandController : Controller
    {
        private readonly DataContext _dataContext;
        public BrandController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index(string Slug = "", string sort_by = "")
        {
            BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();

            if (brand == null) return RedirectToAction("Index");

            var productsByBrand = _dataContext.Products.Where(p => p.BrandID == brand.Id);
            var count = await productsByBrand.CountAsync();
            if (count > 0)
            {
                // Apply sorting based on sort_by parameter
                if (sort_by == "price_increase")
                {
                    productsByBrand = productsByBrand.OrderBy(p => p.Price);
                }
                else if (sort_by == "price_decrease")
                {
                    productsByBrand = productsByBrand.OrderByDescending(p => p.Price);
                }
                else if (sort_by == "price_newest")
                {
                    productsByBrand = productsByBrand.OrderByDescending(p => p.Id);
                }
                else if (sort_by == "price_oldest")
                {
                    productsByBrand = productsByBrand.OrderBy(p => p.Id);
                }
            }

            return View(await productsByBrand.ToListAsync());
        }
    }
}
