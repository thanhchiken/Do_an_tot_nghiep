using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "nhap UserName")]
        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "nhap password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
