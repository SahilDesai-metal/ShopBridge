using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.Tests.ProductMocks
{
    public class ProductMockObject
    {
        public static Product NewProduct
            => new Product { Name = "P1", Description = "D1", Price = 10, Quantity = 10 };

        public static ProductUpdateDto NewProductUpdateDto
            => new ProductUpdateDto { Name = "Neww", ProductDescription = "D1", AvailableQuantity = 10, MRP = 10 };

        public static ProductCreationDto NewProductCreateDto
            => new ProductCreationDto { Name = "Neww", ProductDescription = "D1", AvailableQuantity = 10, MRP = 10 };
    }
}
