using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models;
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
                   new PagingParameters(1, 100),
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
                    pageSize = 100,
                    total = 100
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
                await DeleteImage(existItem.BannerImage);
                existItem.BannerImage = postData.bannerImage;
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
    }
}

