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
        private readonly ILogger<NewController> _logger;
        private readonly INewService _newService;
        public NewController(
          ILogger<NewController> logger, INewService newService)
        {
            _logger = logger;
            _newService = newService;
        }


        [HttpGet("GetNewsList", Name = "GetNewsList")]
        public async Task<IActionResult> Get()
        {
            var result = await _newService.GetNewsList();
            return Ok(result);
        }

        [HttpPost("Update", Name = "UpdateNew")]
        public async Task<Acknowledgement> Update([FromBody] NewsResponseModel postData)
        {
            return await _newService.Update(postData);
        }

        [HttpGet("FindById")]
        public async Task<Acknowledgement<NewReponseModel>> GetUserById(int newId)
        {
            var ack = await _newService.GetUserById(newId);
            return ack;
        }
    }
}
