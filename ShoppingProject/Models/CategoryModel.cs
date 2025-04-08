using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="ycau nhap ten danh muc")]
        public string Name { get; set; }
        [Required(ErrorMessage = "ycau nhap ten danh muc")]
        public string Description { get; set; }
        public string Slug { get; set; }
        public int Status { get; set; }
    }
}
