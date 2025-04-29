using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.Common.Util;
using Store.DAL.Services.Interfaces;
using Store.DAL.Services.WebServices;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Search;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BannerController : Controller
    {
        private readonly ILogger<BannerController> _logger;
        private readonly IBannerService _bannerService;
        public BannerController(
          ILogger<BannerController> logger, IBannerService bannerService)
        {
            _logger = logger;
            _bannerService = bannerService;
        }


        [HttpGet(Name = "GetBannerList")]
        public async Task<IActionResult> Get([FromQuery] BannerSearchModel searchModel)
        {
            var result = await _bannerService.GetBannerList(searchModel);
            return Ok(result);
        }

        [HttpPost("Update", Name = "UpdateBanner")]
        public async Task<Acknowledgement> Update([FromBody] BannerRequestModel postData)
        {
            if(postData.uploadFile != null)
            {
                var listIFromFile = Utils.ConvertBase64ListToFormFile(postData.uploadFile);
                postData.listUploadFiles = listIFromFile;
            }
            
            return await _bannerService.Update(postData);
        }

        [HttpGet("FindById")]
        public async Task<Acknowledgement> FindById(int bannerId)
        {
            return await _bannerService.FindById(bannerId);
        }
    }
}
