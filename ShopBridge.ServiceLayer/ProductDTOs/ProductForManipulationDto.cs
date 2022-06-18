using System.ComponentModel.DataAnnotations;

namespace ShopBridge.ServiceLayer.ProductDTOs
{
    public abstract class ProductForManipulationDto
    {
        [Required(ErrorMessage = "Name of Product is required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description of Product is required field")]
        public string ProductDescription { get; set; }
        
        [Required(ErrorMessage = "Price of Product is required field")]
        public double MRP { get; set; }
        
        public int AvailableQuantity { get; set; }
    }
}
