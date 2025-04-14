using Microsoft.AspNetCore.Mvc;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        public CategoryController(
          ILogger<CategoryController> logger, ICategoryRepository categoryRepository, ICategoryService categoryService) 
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }
      
       
        [HttpGet(Name = "GetCategoryList")]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetTenantList();
            return Ok(result);
        }
    }
}
