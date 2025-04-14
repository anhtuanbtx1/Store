using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Respone;

namespace Store.DAL.Services.WebServices
{
    public class CategoryService : BaseService<CategoryService>, ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(
         ILogger<CategoryService> logger,
         IHttpContextAccessor httpContextAccessor,
         IConfiguration configuration,
         ICategoryRepository categoryRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Acknowledgement<JsonResultPaging<List<CategoryResponeModel>>>> GetTenantList()
        {
            var response = new Acknowledgement<JsonResultPaging<List<CategoryResponeModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Category>(true);
                var tennantDbList = await _categoryRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<CategoryResponeModel>>(tennantDbList.Data);
                response.Data = new JsonResultPaging<List<CategoryResponeModel>>()
                {
                    Data = data,
                    PageNumber = 1,
                    PageSize = 100,
                    Total = 100
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.ExtractMessage(ex);
                _logger.LogError("GetUserList " + ex.Message);
                return response;
            }
        }
    }
}
