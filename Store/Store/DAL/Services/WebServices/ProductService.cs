using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Search;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using System.Text.Json;

namespace Store.DAL.Services.WebServices
{
    public class ProductService : BaseService<ProductService>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public ProductService(
         ILogger<ProductService> logger,
         IHttpContextAccessor httpContextAccessor,
         IConfiguration configuration,
         IProductRepository productRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public List<string> GetImageFileUrl(List<string> rawUrl)
        {
            var prefixUrl = Configuration.GetSection("FTP:Host").Value;
            List<string> result = new List<string>();
            foreach ( var url in rawUrl)
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
                if(!string.IsNullOrEmpty(searchModel.searchStringCode))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchStringCode.Trim().ToLower());
                    predicate = predicate.And(i => (
                                                   i.ProductSeriesCode.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                   i.ProductStatusCode.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                   i.ProductSpaceCode.Trim().ToLower().Contains(searchStringNonUnicode) ||
                                                   i.ProductColorCode.Trim().ToLower().Contains(searchStringNonUnicode)
                                                   )
                                            );
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

                ack.Data = _mapper.Map<ProductResponseModel>(product);
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

            List<string> imagePaths = new List<string>
{
    "/Image/warrantyImage/2025_4/87e569b5-5167-4f10-83e3-33a4f3614e0f.png",
    "/Image/warrantyImage/2025_4/123dcc1a-b6ba-435b-95db-4b3851faf4d9.png",
    "/Image/warrantyImage/2025_4/865c4dc6-4bc2-4ca9-a802-375aef1731a0.png"
};
            string jsonString = JsonSerializer.Serialize(imagePaths);

           

            if (postData.productId == 0)
            {
                var newProduct = _mapper.Map<Product>(postData);
                newProduct.ProductImage = jsonString;
                newProduct.ProductColorCode = postData.productColorCode;
                newProduct.ProductColorName = postData.productColorName;
                newProduct.ProductDetail = postData.productDetail ?? "";
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
                    existItem.ProductImage = jsonString;
                    existItem.ProductColorCode = postData.productColorCode;
                    existItem.ProductColorName = postData.productColorName;
                    existItem.ProductDetail = postData.productDetail ?? "";
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
                    await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _productRepository.Repository);
                }
            }
            return ack;
        }

    }
}
