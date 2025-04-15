using Microsoft.AspNetCore.Mvc;
using Store.DAL.Services.Interfaces;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(
          ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }


        [HttpGet("GetProductList", Name = "GetProductList")]
        public async Task<IActionResult> Get()
        {
            var result = await _productService.GetProductList();
            return Ok(result);
        }
    }
}
