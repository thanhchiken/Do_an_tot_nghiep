using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models.ViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập bình luận")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }
}
