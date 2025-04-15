using Microsoft.AspNetCore.Mvc;
using Store.Common.Helper;
using Store.Models;
using Store.Models.Image;
using static Store.Enum.EnumResponse;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageController( ILogger<ImageController> logger, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<HTTPResponseModel> UploadImage([FromForm] UploadImageModel model)
        {
            try
            {
                List<string> imagesResponse = new List<string>();

                if (model.Image != null && model.Image.Count > 0)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath; 
                    var now = DateTime.Now;
                    var folderPath = Path.Combine(webRootPath, "Image", "warrantyImage", $"{now.Year}_{now.Month}");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    _logger.LogInformation("UploadImage << SaveLocalFile Begin: " + DateTime.Now);

                    foreach (var file in model.Image)
                    {
                        //var fileName = $"{now.Ticks}{Path.GetExtension(file.FileName)}";
                        string fileName = $"{Helper.GenerateUUID()}{Path.GetExtension(file.FileName)}";
                        var filePath = Path.Combine(folderPath, fileName);

                        // Lưu file vào thư mục
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Đường dẫn tương đối tới hình ảnh đã lưu
                        var relativePath = Path.Combine("Image", "warrantyImage", $"{now.Year}_{now.Month}", fileName);
                        imagesResponse.Add(Path.Combine("/", relativePath).Replace("\\", "/"));
                    }

                    _logger.LogInformation("Notification UploadImage << SaveLocalFile End: " + DateTime.Now);
                    return HTTPResponseModel.Make(REPONSE_ENUM.RS_OK, "Successful", null, imagesResponse);
                }
                else
                {
                    _logger.LogError("Notification Insert Image : image null");
                    throw new Exception("Invalid input data !");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Notification UploadImage: " + ex.Message);
                return HTTPResponseModel.Make(REPONSE_ENUM.RS_EXCEPTION, ex.Message);
            }
        }
    }
}
