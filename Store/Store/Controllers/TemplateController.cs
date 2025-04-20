using Microsoft.AspNetCore.Mvc;
using Store.DAL.Services.Interfaces;
using Store.Models.Search;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ILogger<TemplateController> _logger;
        private readonly ITemplateService _templateService;
        public TemplateController(
          ILogger<TemplateController> logger, ITemplateService templateService)
        {
            _logger = logger;
            _templateService = templateService;
        }


        [HttpGet("GetTemplateList", Name = "GetTemplateList")]
        public async Task<IActionResult> Get([FromQuery] TemplateSearchModel searchModel)
        {
            var result = await _templateService.GetTemplateList(searchModel);
            return Ok(result);
        }
    }
}
