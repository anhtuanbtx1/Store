using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Models.Search;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public AccountController(
         ILogger<AccountController> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("Login", Name = "Login")]
        public async Task<Acknowledgement> Get([FromQuery] string key)
        {
            var ack = new Acknowledgement();
            ack.IsSuccess = true;
            ack.AddSuccessMessages("Login thành công");
            var existItem = await _categoryRepository.Repository.FirstOrDefaultAsync(i=>i.CategoryName == key);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy người dùng");
                ack.IsSuccess = false;
                return ack;
            }
            return ack;
        }
    }
}
