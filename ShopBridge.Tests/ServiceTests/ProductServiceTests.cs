using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopBridge.DataLayer;
using ShopBridge.ServiceLayer.ProductService;
using ShopBridge.Tests.ProductMocks;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ShopBridge.Tests.ServiceTests
{
    public class ProductServiceTests
    {
        private static DbContextOptions<ApplicationDbContext> NewContext()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MyBlogDb")
                .UseInternalServiceProvider(new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider());

            return builder.Options;
        }

        [Fact]
        public async Task CheckProductWhenCreatedSuccessfully()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var newProduct = ProductMockObject.NewProduct;

            await service.CreateProductAsync(newProduct);

            Assert.Equal(1, await context.Products.CountAsync());
        }
        
        [Fact]
        public async Task ReturnFalseIfGuidNotFound()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            Assert.False(await service.ProductExistsAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task ThrowsErrorIfGuidIsEmptyWhenCheckingProductExists()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ProductExistsAsync(Guid.Empty));
        }

        [Fact]
        public async Task ReturnProductIfCorrectGuidForProductExists()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var newProduct = ProductMockObject.NewProduct;

            await service.CreateProductAsync(newProduct);

            Assert.Equal(1, await context.Products.CountAsync());
            Assert.True(await service.ProductExistsAsync(newProduct.Id));
        }

        [Fact]
        public async Task CheckDBAfterDeleteProduct()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var newProduct = ProductMockObject.NewProduct;

            await service.CreateProductAsync(newProduct);

            Assert.Equal(1, await context.Products.CountAsync());

            await service.DeleteProductAsync(newProduct);

            Assert.Equal(0, await context.Products.CountAsync());
        }

        [Fact]
        public async Task CheckListOfProducts()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var newProduct = ProductMockObject.NewProduct;

            await service.CreateProductAsync(newProduct);

            Assert.Single(await service.ListProductsAsync());
        }

        [Fact]
        public async Task ThrowErrorForEmptyGuidForFetchProduct()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetProductAsync(Guid.Empty));
        }

        [Fact]
        public async Task FetchProductIfCorrectGuid()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var newProduct = ProductMockObject.NewProduct;

            await service.CreateProductAsync(newProduct);

            Assert.Equal(JsonConvert.SerializeObject(newProduct), JsonConvert.SerializeObject(await service.GetProductAsync(newProduct.Id)));
        }

        [Fact]
        public async Task ReturnNullIfInCorrectGuidForFetchProduct()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            Assert.Null(await service.GetProductAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateDbProductWithNewProduct()
        {
            var context = new ApplicationDbContext(NewContext());

            IProductService service = new ProductService(context);

            var product = ProductMockObject.NewProduct;

            await service.CreateProductAsync(product);

            var newProduct = ProductMockObject.NewProductUpdateDto;

            await service.UpdateProductAsync(product, newProduct);
            
            Assert.Equal(newProduct.Name, (await service.GetProductAsync(product.Id)).Name);
        }
    }
}
