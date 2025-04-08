using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="nhap UserName")]
        public string Username { get; set; }
        [Required(ErrorMessage = "nhap Emal"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="nhap password")]
        public string Password { get; set; }
    }
}
