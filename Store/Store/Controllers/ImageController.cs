using Microsoft.AspNetCore.Mvc;
using Store.Common.Helper;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models;
using Store.Models.Image;
using Store.Models.Respone;
using System.Text.Json;
using static Store.Enum.EnumResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository _productRepository;
        public ImageController( ILogger<ImageController> logger, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IProductRepository productRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
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
        [HttpPost("DeleteImage")]
        public async Task<HTTPResponseModel> DeleteImage(int id)
        {
            try
            {
                // Tìm hình ảnh trong cơ sở dữ liệu
                var data = await _productRepository.Repository.FirstOrDefaultAsync(i => i.ProductId == id);
                List<string> listImage = JsonSerializer.Deserialize<List<string>>(data.ProductImage);
                if (listImage.Count < 0)
                {
                    _logger.LogError("Image not found: " + id);
                    return HTTPResponseModel.Make(REPONSE_ENUM.RS_NOT_FOUND, "Image does not exist !");
                }
                foreach (var image in listImage)
                {
                    // Xóa tệp hình ảnh từ thư mục
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    // Loại bỏ dấu gạch chéo đầu tiên từ đường dẫn liên kết hình ảnh
                    var relativePath = image.TrimStart('/');
                    // Kết hợp đường dẫn gốc với đường dẫn tương đối để tạo thành đường dẫn tuyệt đối
                    var filePath = Path.Combine(webRootPath, relativePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

               

                _logger.LogInformation("Image deleted successfully: " + id);
                return HTTPResponseModel.Make(REPONSE_ENUM.RS_OK, "Image deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteImage: " + ex.Message);
                return HTTPResponseModel.Make(REPONSE_ENUM.RS_EXCEPTION, ex.Message);
            }
        }
    }
}
