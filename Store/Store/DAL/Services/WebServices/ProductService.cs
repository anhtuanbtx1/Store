using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Request;
using Store.Models.Respone;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

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

        public async Task<Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>> GetProductList()
        {
            var response = new Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Product>(true);
                var tennantDbList = await _productRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<ProductResponseModel>>(tennantDbList.Data);
                response.Data = new JsonResultPaging<List<ProductResponseModel>>()
                {
                    Data = data,
                    PageNumber = 1,
                    PageSize = 10,
                    Total = 10
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

            if (postData.ProductId == 0)
            {
                var newProduct = _mapper.Map<Product>(postData);
                newProduct.ProductImage = postData.ProductImage;
                newProduct.ProductColorCode = postData.ProductColorCode;
                newProduct.ProductColorName = postData.ProductColorName;
                newProduct.ProductDetail = postData.ProductDetail ?? "";
                newProduct.ProductName = postData.ProductName ?? "";
                newProduct.ProductStatusName = postData.ProductStatusName ?? "";
                newProduct.ProductStatusCode = postData.ProductStatusCode ?? "";
                newProduct.ProductSeriesCode = postData.ProductSeriesCode ?? "";
                newProduct.ProductSeriesName = postData.ProductSeriesName ?? "";
                newProduct.ProductSpaceCode = postData.ProductSpaceCode ?? "";
                newProduct.ProductSpaceName = postData.ProductSpaceName ?? "";
                newProduct.ProductPrice = postData.ProductPrice ?? "";
                newProduct.ProductPriceSale = postData.ProductPriceSale ?? "";
                newProduct.CreatedAt = DateTime.Now;
                newProduct.UpdatedAt = DateTime.Now;
              
                await ack.TrySaveChangesAsync(res => res.AddAsync(newProduct), _productRepository.Repository);
            }
            else
            {
                var existItem = await _productRepository.Repository.FirstOrDefaultAsync(i => i.ProductId == postData.ProductId);
                if (existItem == null)
                {
                    ack.AddMessage("Không tìm thấy sản phẩm");
                    ack.IsSuccess = false;
                    return ack;
                }
                else
                {
                    existItem.ProductImage = postData.ProductImage;
                    existItem.ProductColorCode = postData.ProductColorCode;
                    existItem.ProductColorName = postData.ProductColorName;
                    existItem.ProductDetail = postData.ProductDetail ?? "";
                    existItem.ProductName = postData.ProductName ?? "";
                    existItem.ProductStatusName = postData.ProductStatusName ?? "";
                    existItem.ProductStatusCode = postData.ProductStatusCode ?? "";
                    existItem.ProductSeriesCode = postData.ProductSeriesCode ?? "";
                    existItem.ProductSeriesName = postData.ProductSeriesName ?? "";
                    existItem.ProductSpaceCode = postData.ProductSpaceCode ?? "";
                    existItem.ProductSpaceName = postData.ProductSpaceName ?? "";
                    existItem.ProductPrice = postData.ProductPrice ?? "";
                    existItem.ProductPriceSale = postData.ProductPriceSale ?? "";
                    existItem.UpdatedAt = DateTime.Now;
                    await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _productRepository.Repository);
                }
            }
            return ack;
        }

    }
}
