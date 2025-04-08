using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models
{
    public class BrandModel
    {
        [Key]
        public int Id { get; set; }
        [Required( ErrorMessage = "ycau nhap ten thuong hieu")]
        public string Name { get; set; }
        [Required(ErrorMessage = "ycau nhap mo ta thuong hieu")]
        public string Description { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
