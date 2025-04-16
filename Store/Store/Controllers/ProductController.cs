using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.DAL.Services.Interfaces;
using Store.DAL.Services.WebServices;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Search;

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
        public async Task<IActionResult> Get([FromQuery] ProductSearchModel searchModel)
        {
            var result = await _productService.GetProductList(searchModel);
            return Ok(result);
        }

        [HttpPost("CreateOrUpdate", Name = "CreateOrUpdate")]
        public async Task<Acknowledgement> CreateOrUpdate([FromBody] ProductRequestModel postData)
        {
            return await _productService.CreateOrUpdate(postData);
        }

        [HttpGet("FindById")]
        public async Task<Acknowledgement<ProductResponseModel>> GetById(int productId)
        {
            var ack = await _productService.GetById(productId);
            return ack;
        }
    }
}
