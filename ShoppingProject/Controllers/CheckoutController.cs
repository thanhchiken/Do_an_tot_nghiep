﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if(userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;

                orderItem.UserName = userEmail;
                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cart in cartItems)
                {
                    var orderdetails = new OrderDetails
                    {
                        UserName = userEmail,
                        OrderCode = ordercode,
                        Product = _dataContext.Products.Find((int)cart.ProductId), // Chỉ gán Product
                        Price = cart.Price,
                        Quantity = cart.Quantity
                    };
                    _dataContext.Add(orderdetails);
                }
                _dataContext.SaveChanges();

                HttpContext.Session.Remove("Cart");
                TempData["success"] = "thanh cong checkout";
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

    }
}
