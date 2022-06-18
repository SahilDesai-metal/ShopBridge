using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShopBridge.DataLayer.ProductEntity;
using ShopBridge.ServiceLayer.ProductDTOs;
using ShopBridge.ServiceLayer.ProductService;

namespace ShopBridge.API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet, Route("[controller]/all")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> FetchAllProduct()
        {
            return Ok(await _service.ListProductsAsync());
        }

        [HttpGet, Route("[controller]/{id:Guid}")]
        public async Task<IActionResult> FetchProduct([FromRoute] Guid id)
        {
            var product = await _service.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductCreationDto>(product));
        }

        [HttpPost("[controller]/create", Name = "CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductCreationDto newProduct)
        {
            var product = _mapper.Map<Product>(newProduct);

            await _service.CreateProductAsync(product);

            return CreatedAtRoute(nameof(CreateProduct), product);
        }

        [HttpPut("[controller]/edit/{productId:Guid}", Name = "EditProduct")]
        public async Task<IActionResult> EditProduct(Guid productId, ProductUpdateDto newProduct)
        {
            var currentProductFromRepo = await _service.GetProductAsync(productId);

            if (currentProductFromRepo != null)
            {
                await _service.UpdateProductAsync(currentProductFromRepo, newProduct);
                return NoContent();
            }

            var product = _mapper.Map<Product>(newProduct);

            await _service.CreateProductAsync(product);

            return CreatedAtRoute(nameof(EditProduct), new { newProductId = productId }, product);
        }

        [HttpDelete, Route("[controller]/delete/{productId:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            var productFromRepo = await _service.GetProductAsync(productId);

            if (productFromRepo == null) return NotFound();

            await _service.DeleteProductAsync(productFromRepo);

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelState)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
