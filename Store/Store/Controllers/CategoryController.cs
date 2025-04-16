using Microsoft.AspNetCore.Mvc;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Models.Search;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(
          ILogger<CategoryController> logger, ICategoryService categoryService) 
        {
            _logger = logger;
            _categoryService = categoryService;
        }
      
       
        [HttpGet(Name = "GetCategoryList")]
        public async Task<IActionResult> Get([FromQuery] CategorySearchModel searchModel)
        {
            var result = await _categoryService.GetCategoryList(searchModel);
            return Ok(result);
        }
    }
}
