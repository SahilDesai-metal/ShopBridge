using Microsoft.EntityFrameworkCore;
using ShopBridge.DataLayer;
using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.ServiceLayer.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ProductExistsAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }    
            
            return await _context.Products.AnyAsync(x => x.Id == productId);
        }

        public async Task CreateProductAsync(Product newProduct)
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> ListProductsAsync()
            => await _context.Products.ToListAsync();

        public async Task UpdateProductAsync(Product existingProduct, ProductUpdateDto newProduct)
        {
            existingProduct.Name = newProduct.Name;
            existingProduct.Price = newProduct.MRP;
            existingProduct.Quantity = newProduct.AvailableQuantity;
            existingProduct.Description = newProduct.ProductDescription;
            
            _context.Products.Update(existingProduct);
            
            await _context.SaveChangesAsync();
        }
    }
}