using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models;
using Store.Models.Image;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Response;
using Store.Models.Search;
using System.Security.Policy;
using System.Text.Json;
using static Store.Enum.EnumResponse;

namespace Store.DAL.Services.WebServices
{
    public class BannerService : BaseService<BannerService>, IBannerService
    {
        private readonly IMapper _mapper;
        private readonly IBannerRepository _bannerRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BannerService(
         ILogger<BannerService> logger,
         IHttpContextAccessor httpContextAccessor,
         IWebHostEnvironment webHostEnvironment,
         IConfiguration configuration,
         IBannerRepository bannerRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _bannerRepository = bannerRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public string GetImageFileUrl(string rawUrl)
        {
            var prefixUrl = Configuration.GetSection("FTP:Host").Value;
            return prefixUrl + rawUrl;
        }
        public async Task<Acknowledgement<JsonResultPaging<List<BannerResponseModel>>>> GetBannerList(BannerSearchModel searchModel)
        {
            var response = new Acknowledgement<JsonResultPaging<List<BannerResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Banner>(true);

                if (!string.IsNullOrEmpty(searchModel.searchCode))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchCode.Trim().ToLower());
                    predicate = predicate.And(i => (i.BannerCode.Trim().ToLower().Contains(searchStringNonUnicode)));
                }

                if (!string.IsNullOrEmpty(searchModel.searchType))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchType.Trim().ToLower());
                    predicate = predicate.And(i => (i.BannerTypeCode.Trim().ToLower().Contains(searchStringNonUnicode)));
                }

                var dbList = await _bannerRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 10),
                   predicate
                   );
                var data = _mapper.Map<List<BannerResponseModel>>(dbList.Data);
                data.ForEach(i =>
                {
                    if (!string.IsNullOrWhiteSpace(i.bannerImage))
                    {
                        i.bannerImage = GetImageFileUrl(i.bannerImage);
                    }
                });
                response.Data = new JsonResultPaging<List<BannerResponseModel>>()
                {
                    data = data,
                    pageNumber = 1,
                    pageSize = dbList.PageSize,
                    total = dbList.TotalRecords
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ExtractMessage(ex);
                _logger.LogError("GetBannerList " + ex.Message);
                return response;
            }
        }

        public async Task<Acknowledgement> Update(BannerRequestModel postData)
        {
            var ack = new Acknowledgement();
            var existItem = await _bannerRepository.Repository.FirstOrDefaultAsync(i => i.BannerId == postData.bannerId);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy banner");
                ack.IsSuccess = false;
                return ack;
            }
            else
            {
                if (postData.uploadFile != null)
                {
                    var index = existItem.BannerImage.IndexOf("/Image", StringComparison.OrdinalIgnoreCase);
                    if (index >= 0)
                    {
                       var item = existItem.BannerImage.Substring(index);
                        _ = DeleteImage(item);
                    }
                    var listPath = await UploadImage(postData.listUploadFiles);
                    
                    existItem.BannerImage = listPath[0];
                }
                existItem.BannerName = postData.bannerName;
                existItem.BannerTitle = postData.bannerTitle;
                existItem.BannerSubTitle = postData?.bannerSubTitle;
                existItem.UpdatedAt = DateTime.Now;

                await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _bannerRepository.Repository);
            }


            return ack;
        }

        public async Task<Acknowledgement> FindById(int bannerId)
        {
            var ack = new Acknowledgement<BannerResponseModel>();
            try
            {
                var banner = await _bannerRepository.ReadOnlyRespository.FindAsync(bannerId);
                if (banner == null)
                {
                    ack.IsSuccess = false;
                    ack.AddMessages("Không tìm thấy banner");
                    return ack;
                }
                banner.BannerImage = GetImageFileUrl(banner.BannerImage);

                ack.Data = _mapper.Map<BannerResponseModel>(banner);
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

