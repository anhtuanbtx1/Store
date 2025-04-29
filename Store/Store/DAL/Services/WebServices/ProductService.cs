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
using Store.Models.Search;
using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text.Json;
using static Store.Enum.EnumResponse;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Store.DAL.Services.WebServices
{
    public class ProductService : BaseService<ProductService>, IProductService 
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(
         ILogger<ProductService> logger,
         IHttpContextAccessor httpContextAccessor,
         IConfiguration configuration,
         IWebHostEnvironment webHostEnvironment,
         IProductRepository productRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
        }
        public List<string> GetImageFileUrl(List<string> rawUrl)
        {
            var prefixUrl = Configuration.GetSection("FTP:Host").Value;
            List<string> result = new List<string>();
            foreach (var url in rawUrl)
            {
                var item = prefixUrl + url;
                result.Add(item);
            }
            return result;
        }
        public async Task<Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>> GetProductList(ProductSearchModel searchModel)
        {
            var response = new Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Product>(true);

                if (!string.IsNullOrEmpty(searchModel.searchString))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchString.Trim().ToLower());
                    predicate = predicate.And(i => (
                                                    i.ProductSeriesName.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                    i.ProductStatusName.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                    i.ProductSpaceName.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                    i.ProductColorName.Trim().ToLower().Contains(searchStringNonUnicode)
                                                    )
                                             );
                }
                if (!string.IsNullOrEmpty(searchModel.seriCode))
                {
                    if (searchModel.seriCode != "ALL")
                    {
                        var searchSeriCode = Utils.NonUnicode(searchModel.seriCode.Trim().ToLower());
                        predicate = predicate.And(i => (i.ProductSeriesCode.Trim().ToLower().Contains(searchSeriCode)));
                    }
                   
                }
                if (!string.IsNullOrEmpty(searchModel.spaceCode))
                {
                    if (searchModel.spaceCode != "ALL")
                    {
                        var searchSpaceCode = Utils.NonUnicode(searchModel.spaceCode.Trim().ToLower());
                        predicate = predicate.And(i => (i.ProductSpaceCode.Trim().ToLower().Contains(searchSpaceCode)));
                    }
                  
                }
                if (!string.IsNullOrEmpty(searchModel.colorCode))
                {
                    if (searchModel.colorCode != "ALL")
                    {
                        var searchColorCode = Utils.NonUnicode(searchModel.spaceCode.Trim().ToLower());
                        predicate = predicate.And(i => (i.ProductColorCode.Trim().ToLower().Contains(searchColorCode)));
                    }
                    
                }
                if (searchModel.statusCodes != null)
                {
                    if(searchModel.statusCodes.Count > 0)
                    {
                        if (!searchModel.statusCodes.Contains("ALL"))
                        {
                            predicate = predicate.And(i => (searchModel.statusCodes.Contains(i.ProductStatusCode)));
                        }
                    }
                }

                var dbList = await _productRepository.ReadOnlyRespository.GetWithPagingAsync(
                     new PagingParameters(searchModel.pageNumber, searchModel.pageSize),
                   predicate, i => i.OrderByDescending(p => p.UpdatedAt)
                   );
                var data = _mapper.Map<List<ProductResponseModel>>(dbList.Data);
                data.ForEach(i =>
                {
                    if (!string.IsNullOrWhiteSpace(i.ProductImage))
                    {
                        List<string> imagePaths = JsonSerializer.Deserialize<List<string>>(i.ProductImage);
                        i.ListImage = GetImageFileUrl(imagePaths);
                    }
                    else
                    {
                        i.ProductImage = "../content/images/noImage.png";
                    }
                });

                response.Data = new JsonResultPaging<List<ProductResponseModel>>()
                {
                    data = data,
                    pageNumber = searchModel.pageNumber,
                    pageSize = dbList.PageSize,
                    total = dbList.TotalRecords
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

        public async Task<Acknowledgement<ProductResponseModel>> GetById(int productId)
        {
            var ack = new Acknowledgement<ProductResponseModel>();
            try
            {
                var product = await _productRepository.ReadOnlyRespository.FindAsync(productId);
                if (product == null)
                {
                    ack.IsSuccess = false;
                    ack.AddMessages("Không tìm thấy sản phẩm");
                    return ack;
                }

                var data = _mapper.Map<ProductResponseModel>(product);
                List<string> imagePaths = JsonSerializer.Deserialize<List<string>>(data.ProductImage);
                data.ListImage = GetImageFileUrl(imagePaths);
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

        public async Task<Acknowledgement> CreateOrUpdate(ProductRequestModel postData)
        {
            var ack = new Acknowledgement();
          
            var newProduct = _mapper.Map<Product>(postData);
            if (postData.productId == 0)
            {
                newProduct.ProductColorCode = postData.productColorCode;
                newProduct.ProductColorName = postData.productColorName;
                newProduct.ProductDetail = postData.productDetail ?? "";
                newProduct.ProductShortDetail = postData.productShortDetail ?? "";
                newProduct.ProductName = postData.productName ?? "";
                newProduct.ProductStatusName = postData.productStatusName ?? "";
                newProduct.ProductStatusCode = postData.productStatusCode ?? "";
                newProduct.ProductSeriesCode = postData.productSeriesCode ?? "";
                newProduct.ProductSeriesName = postData.productSeriesName ?? "";
                newProduct.ProductSpaceCode = postData.productSpaceCode ?? "";
                newProduct.ProductSpaceName = postData.productSpaceName ?? "";
                newProduct.ProductPrice = postData.productPrice ?? "";
                newProduct.ProductPriceSale = postData.productPriceSale ?? "";
                newProduct.CreatedAt = DateTime.Now;
                newProduct.UpdatedAt = DateTime.Now;
                if (postData.uploadFiles != null || postData.uploadFiles.Count > 0)
                {
                    var listPath = await UploadImage(postData.listUploadFiles);
                    string jsonString = JsonSerializer.Serialize(listPath);
                    newProduct.ProductImage = jsonString;
                }

                await ack.TrySaveChangesAsync(res => res.AddAsync(newProduct), _productRepository.Repository);
            }
            else
            {
                var existItem = await _productRepository.Repository.FirstOrDefaultAsync(i => i.ProductId == postData.productId);
                if (existItem == null)
                {
                    ack.AddMessage("Không tìm thấy sản phẩm");
                    ack.IsSuccess = false;
                    return ack;
                }
                else
                {
                    existItem.ProductColorCode = postData.productColorCode;
                    existItem.ProductColorName = postData.productColorName;
                    existItem.ProductDetail = postData.productDetail ?? "";
                    existItem.ProductShortDetail = postData.productShortDetail ?? "";
                    existItem.ProductName = postData.productName ?? "";
                    existItem.ProductStatusName = postData.productStatusName ?? "";
                    existItem.ProductStatusCode = postData.productStatusCode ?? "";
                    existItem.ProductSeriesCode = postData.productSeriesCode ?? "";
                    existItem.ProductSeriesName = postData.productSeriesName ?? "";
                    existItem.ProductSpaceCode = postData.productSpaceCode ?? "";
                    existItem.ProductSpaceName = postData.productSpaceName ?? "";
                    existItem.ProductPrice = postData.productPrice ?? "";
                    existItem.ProductPriceSale = postData.productPriceSale ?? "";
                    existItem.UpdatedAt = DateTime.Now;

                    var paths = postData.listImage.Select(url =>
                    {
                        var index = url.IndexOf("/Image", StringComparison.OrdinalIgnoreCase);
                        return index >= 0 ? url.Substring(index) : url;
                    }).ToList();
                    List<string> imagePaths = JsonSerializer.Deserialize<List<string>>(existItem.ProductImage);
                    var onlyInList1 = imagePaths.Except(paths).ToList();
                    if(onlyInList1.Count > 0)
                    {
                        foreach (var item in onlyInList1)
                        {
                            _=DeleteImage(item);
                        }
                    }

                    if (postData.uploadFiles.Count > 0)
                    {
                        var listPath = await UploadImage(postData.listUploadFiles);
                        paths.AddRange(listPath);
                        string jsonString = JsonSerializer.Serialize(paths);
                        existItem.ProductImage = jsonString;
                    }
                    else
                    {
                        string jsonString = JsonSerializer.Serialize(paths);
                        existItem.ProductImage = jsonString;
                    }
                    await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _productRepository.Repository);
                }
            }
            return ack;
        }

        public async Task<Acknowledgement> DeleteById(int userId)
        {
            var ack = new Acknowledgement();
            var existItem = await _productRepository.Repository.FirstOrDefaultAsync(i => i.ProductId == userId);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy sản phẩm");
                return ack;
            }
            DeleteImage(existItem.ProductImage);
            await _productRepository.Repository.DeleteAsync(userId);
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
