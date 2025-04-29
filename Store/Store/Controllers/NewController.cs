using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Models.Request;
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
        public async Task<Acknowledgement> Update([FromBody] NewRequestModel postData)
        {
            if (postData.uploadFile != null)
            {
                var listIFromFile = Utils.ConvertBase64ListToFormFile(postData.uploadFile);
                postData.listUploadFiles = listIFromFile;
            }
            return await _newService.Update(postData);
        }

        [HttpGet("FindById")]
        public async Task<Acknowledgement<NewResponseModel>> GetById(int newId)
        {
            var ack = await _newService.GetById(newId);
            return ack;
        }


    }
}
