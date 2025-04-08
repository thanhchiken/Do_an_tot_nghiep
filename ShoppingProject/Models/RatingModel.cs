using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingProject.Models
{
    public class RatingModel
    {
        [Key]
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        [Required(ErrorMessage ="Nhập tên đánh giá sản phẩm")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Nhập tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        public string Email { get; set; }

        public string Start {  get; set; } // ngôi sao viết lộn á


        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
