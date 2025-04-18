﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingProject.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderCode { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductModel Product { get; set; } // Entity Framework tự xử lý khóa ngoại
    }
}
