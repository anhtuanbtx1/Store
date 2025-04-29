using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Image;
using Store.Models;
using Store.Models.Request;
using Store.Models.Respone;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text.Json;
using static Store.Enum.EnumResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store.DAL.Services.WebServices
{
    public class NewService : BaseService<NewService>, INewService
    {
        private readonly IMapper _mapper;
        private readonly INewRepository _newRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NewService(
         ILogger<NewService> logger,
         IHttpContextAccessor httpContextAccessor,
         IWebHostEnvironment webHostEnvironment,
         IConfiguration configuration,
         INewRepository newRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _newRepository = newRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public string GetImageFileUrl(string rawUrl)
        {
            var prefixUrl = Configuration.GetSection("FTP:Host").Value;
            return prefixUrl + rawUrl;
        }
        public async Task<Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>> GetNewsList()
        {
            var response = new Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<News>(i => i.State == true);
                var tennantDbList = await _newRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<NewsResponseModel>>(tennantDbList.Data);
                data.ForEach(i =>
                {
                    if (!string.IsNullOrWhiteSpace(i.newsThumbnail))
                    {
                        i.newsThumbnail = GetImageFileUrl(i.newsThumbnail);
                    }
                    else
                    {
                        i.newsThumbnail = "../content/images/noImage.png";
                    }
                });
                response.Data = new JsonResultPaging<List<NewsResponseModel>>()
                {
                    data = data,
                    pageNumber = 1,
                    pageSize = 10,
                    total = 10
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ExtractMessage(ex);
                _logger.LogError("GetNewList " + ex.Message);
                return response;
            }
        }

        public async Task<Acknowledgement<NewResponseModel>> GetById(int newId)
        {
            var ack = new Acknowledgement<NewResponseModel>();
            try
            {
                var user = await _newRepository.ReadOnlyRespository.FindAsync(newId);
                if (user == null)
                {
                    ack.IsSuccess = false;
                    ack.AddMessages("Không tìm thấy user");
                    return ack;
                }
                var data = _mapper.Map<NewResponseModel>(user);
                data.newsThumbnail = GetImageFileUrl(data.newsThumbnail);

                ack.Data = data;
                ack.IsSuccess = true;

                return ack;

            }
            catch (Exception ex)
            {
                ack.ExtractMessage(ex);
                _logger.LogError("GetUserList " + ex.Message);
                return ack;
            }
        }

        public async Task<Acknowledgement> Update(NewRequestModel postData)
        {
            var ack = new Acknowledgement();
            var existItem = await _newRepository.Repository.FirstOrDefaultAsync(i => i.NewsId == postData.newsId);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy người dùng");
                ack.IsSuccess = false;
                return ack;
            }
            else
            {
                if (postData.uploadFile != null)
                {
                    var index = existItem.NewsThumbnail.IndexOf("/Image", StringComparison.OrdinalIgnoreCase);
                    if (index >= 0)
                    {
                        var item = existItem.NewsThumbnail.Substring(index);
                        _ = DeleteImage(item);
                    }
                    var listPath = await UploadImage(postData.listUploadFiles);

                    existItem.NewsThumbnail = listPath[0];
                }
                existItem.State = postData.state;
                existItem.NewsTitle = postData.newsTitle;       
                existItem.NewsDetailContent = postData.newsDetailContent;
                existItem.UpdatedAt = DateTime.Now;
                existItem.NewsShortContent = postData.newsShortContent;
             
                await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _newRepository.Repository);
            }


            return ack;
        }

        public async Task<HTTPResponseModel> DeleteImage(string productImage)
        {
            try
            {
                List<string> listImage = JsonSerializer.Deserialize<List<string>>(productImage);
                if (listImage.Count < 0)
                {
                    _logger.LogError("Image not found");
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

                _logger.LogInformation("Image deleted successfully: ");
                return HTTPResponseModel.Make(REPONSE_ENUM.RS_OK, "Image deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteImage: " + ex.Message);
                return HTTPResponseModel.Make(REPONSE_ENUM.RS_NOT_FOUND, "Image does not exist !");
            }
        }

        public async Task<List<string>> UploadImage([FromForm] List<UploadImageModel> model)
        {
            try
            {
                List<string> imagesResponse = new List<string>();

                if (model != null && model.Count > 0)
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

                    foreach (var file in model)
                    {
                        string extension = Utils.GetFileExtensionFromBase64(file.Type);
                        //var fileName = $"{now.Ticks}{Path.GetExtension(file.FileName)}";
                        string fileName = $"{Helper.GenerateUUID()}{Path.GetExtension(file.Image.FileName)}" + file.Type;
                        var filePath = Path.Combine(folderPath, fileName);

                        // Lưu file vào thư mục
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.Image.CopyToAsync(fileStream);
                        }

                        // Đường dẫn tương đối tới hình ảnh đã lưu
                        var relativePath = Path.Combine("Image", "warrantyImage", $"{now.Year}_{now.Month}", fileName);
                        imagesResponse.Add(Path.Combine("/", relativePath).Replace("\\", "/"));
                    }

                    _logger.LogInformation("Notification UploadImage << SaveLocalFile End: " + DateTime.Now);
                    return imagesResponse;
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
                return null;
            }
        }
    }
}
