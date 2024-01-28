using ElasticTest.Models;
using ElasticTest.Services.Products;
using ElasticTest.Services.Products.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElasticTest.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductService productService,
            ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;

        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchProduct(
            [FromBody] SearchProductRequest request)
        {
            var result = await _productService.Search(request);

            return Ok(result);
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadProduct(Product product)
        {
            if (await _productService.IndexProduct(product))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
