using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Models.Respone;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly INewService _newService;
        public NewController(
          ILogger<CategoryController> logger, INewService newService)
        {
            _logger = logger;
            _newService = newService;
        }


        [HttpGet(Name = "GetNewList")]
        public async Task<IActionResult> Get()
        {
            var result = await _newService.GetTenantList();
            return Ok(result);
        }

        [HttpPost(Name = "UpdateNew")]
        public async Task<Acknowledgement> Update([FromBody] NewsResponseModel postData)
        {
            return await _newService.Update(postData);
        }
    }
}
