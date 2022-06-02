using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShopBridge.API.Controllers;
using ShopBridge.DataLayer;
using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductService;
using ShopBridge.Tests.ProductMocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System;
using ShopBridge.ServiceLayer.ProductDTOs;

namespace ShopBridge.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private Mock<IProductService> _service;
        private Mock<IMapper> _mapper;
        private ApplicationDbContext _context;
        private ProductController _controller;

        private static DbContextOptions<ApplicationDbContext> NewContext()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase(databaseName: "MyBlogDb")
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        private enum PrivateStatusCode
        {
            Ok = 200,
            Created = 201,
            NoContent = 204,
            NotFound = 404,
            InternalServerError = 500,
            BadRequest = 400
        }

        private void CleanPreviousTests()
        {
            _mapper = new Mock<IMapper>() { CallBase = true };
            _context = new ApplicationDbContext(NewContext());
            _service = new Mock<IProductService>() { CallBase = true };
            _controller = new ProductController(_service.Object, _mapper.Object);
        }

        [Fact]
        public async Task CheckProductList()
        {
            //Setup
            CleanPreviousTests();
            _service.Setup(x => x.ListProductsAsync())
                .Returns(Task.FromResult(new List<Product> { ProductMockObject.NewProduct }));

            var response = await _controller.FetchAllProduct();

            Assert.Equal((int)PrivateStatusCode.Ok, ((OkObjectResult)response).StatusCode.Value);
        }

        [Fact]
        public async Task ThrowErrorIfProductNotFoundWhenFetchingProduct()
        {
            //Setup
            CleanPreviousTests();
            Product returnedProduct = null;
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(returnedProduct));

            var response = await _controller.FetchProduct(Guid.Empty);

            Assert.Equal((int)PrivateStatusCode.NotFound, ((NotFoundResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnProductDtoIfCorrectGuid()
        {
            //Setup
            CleanPreviousTests();
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(ProductMockObject.NewProduct));
            
            _mapper.Setup(x => x.Map(It.IsAny<Product>(), It.IsAny<ProductCreationDto>()))
                .Returns(ProductMockObject.NewProductCreateDto);
            
            var response = await _controller.FetchProduct(Guid.Empty);

            Assert.Equal((int)PrivateStatusCode.Ok, ((OkObjectResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnCreatedIfNewProduct()
        {
            //Setup
            CleanPreviousTests();
            _service.Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mapper.Setup(x => x.Map(It.IsAny<ProductCreationDto>(), It.IsAny<Product>()))
                .Returns(ProductMockObject.NewProduct);

            var response = await _controller.CreateProduct(ProductMockObject.NewProductCreateDto);

            Assert.Equal((int)PrivateStatusCode.Created, ((CreatedAtRouteResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnNoContentForUpdateRequest()
        {
            //Setup
            CleanPreviousTests();
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(ProductMockObject.NewProduct));
            _service.Setup(x => x.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductUpdateDto>()))
                .Returns(Task.CompletedTask);
            
            var response = await _controller.EditProduct(Guid.Empty, ProductMockObject.NewProductUpdateDto);

            Assert.Equal((int)PrivateStatusCode.NoContent, ((NoContentResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnCreatedIfNewEntityInUpdateRequest()
        {
            //Setup
            CleanPreviousTests();
            Product nullProduct = null;
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(nullProduct));
            _service.Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _mapper.Setup(x => x.Map(It.IsAny<ProductCreationDto>(), It.IsAny<Product>()))
                .Returns(ProductMockObject.NewProduct);

            var response = await _controller.EditProduct(Guid.Empty, ProductMockObject.NewProductUpdateDto);

            Assert.Equal((int)PrivateStatusCode.Created, ((CreatedAtRouteResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnNotFoundIfInvalidGuidForDeleteRequest()
        {
            CleanPreviousTests();
            Product nullProduct = null;
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(nullProduct));

            var response = await _controller.DeleteProduct(Guid.Empty);

            Assert.Equal((int)PrivateStatusCode.NotFound, ((NotFoundResult)response).StatusCode);
        }

        [Fact]
        public async Task ReturnNotContentIfValidGuidForDeleteRequest()
        {
            CleanPreviousTests();
            _service.Setup(x => x.GetProductAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(ProductMockObject.NewProduct));

            _service.Setup(x => x.DeleteProductAsync(ProductMockObject.NewProduct))
                .Returns(Task.CompletedTask);
            
            var response = await _controller.DeleteProduct(Guid.Empty);

            Assert.Equal((int)PrivateStatusCode.NoContent, ((NoContentResult)response).StatusCode);
        }
    }
}
