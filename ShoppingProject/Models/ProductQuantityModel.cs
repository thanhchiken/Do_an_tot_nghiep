using System.ComponentModel.DataAnnotations;

namespace ShoppingProject.Models
{
    public class ProductQuantityModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="ycau ko bo trong SL san pham")]
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
