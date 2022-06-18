using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopBridge.DataLayer.ProductEntity
{
    [Table("Product")]
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        
        [Required]
        public double Price { get; set; }
        
        public int Quantity { get; set; }
    }
}
