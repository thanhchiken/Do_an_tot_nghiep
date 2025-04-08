using Microsoft.EntityFrameworkCore;
using ShoppingProject.Models;

namespace ShoppingProject.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel Macbook = new CategoryModel { Name = "Macbook", Slug = "macbook", Description = "Macbook is the laptop in the word", Status = 1 };
                CategoryModel pc = new CategoryModel { Name = "Pc", Slug = "pc", Description = "Pc is the Brand 2td in the word", Status = 2 };

                BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is the Brand in the word", Status = 1 };
                BrandModel samsung = new BrandModel { Name = "Samsung", Slug = "samsung", Description = "Samsung is the Brand 2td in the word", Status = 2 };
                _context.Products.AddRange(
                    new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Macbook noi best", Image = "1.png", Category = Macbook, Brand = apple, Price = 1234 },
                    new ProductModel { Name = "Pc", Slug = "pc", Description = "Pc noi best", Image = "2.png", Category = pc, Brand = samsung, Price = 123456 }
                );
                _context.SaveChanges();
            }

        }
    }
}