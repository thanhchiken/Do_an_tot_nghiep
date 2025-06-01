using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingProject.Models;
using ShoppingProject.Repository;
using System.Security.Claims;

namespace ShoppingProject.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;

        public CheckoutController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Checkout()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Nhận shipping giá từ cookie
            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;

            if (shippingPriceCookie != null)
            {
                var shippingPriceJson = shippingPriceCookie;
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
            }

            var ordercode = Guid.NewGuid().ToString();
            var orderItem = new OrderModel
            {
                OrderCode = ordercode,
                UserName = userEmail,
                Status = 1,
                CreatedDate = DateTime.Now
            };

            orderItem.ShippingCost = shippingPrice;
            _dataContext.Add(orderItem);
            await _dataContext.SaveChangesAsync();

            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            foreach (var cart in cartItems)
            {
                // Update product quantity
                var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
                product.Quantity -= cart.Quantity;
                product.Sold += cart.Quantity;
                _dataContext.Update(product);

                var orderdetails = new OrderDetails
                {
                    UserName = userEmail,
                    OrderCode = ordercode,
                    Product = product, // Using the fetched product
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };
                _dataContext.Add(orderdetails);
            }

            await _dataContext.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            TempData["success"] = "thanh cong checkout";
            return RedirectToAction("Index", "Cart");
        }
    }
}