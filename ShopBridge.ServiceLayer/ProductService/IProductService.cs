using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.ServiceLayer.ProductService
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(Guid productId);
        Task UpdateProductAsync(Product existingProduct, ProductUpdateDto newProduct);
        Task DeleteProductAsync(Product product);
        Task<List<Product>> ListProductsAsync();
        Task CreateProductAsync(Product newProduct);
        Task<bool> ProductExistsAsync(Guid id);
    }
}
