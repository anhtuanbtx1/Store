using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Respone;

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

        public Task<Acknowledgement<NewReponseModel>> GetUserById(int newId)
        {
            throw new NotImplementedException();
        }

        public Task<Acknowledgement> Update(NewsResponseModel postData)
        {
            throw new NotImplementedException();
        }
    }
}
