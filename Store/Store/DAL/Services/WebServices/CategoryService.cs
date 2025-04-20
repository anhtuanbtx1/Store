using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Respone;
using Store.Models.Search;

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

        public async Task<Acknowledgement<JsonResultPaging<List<CategoryResponseModel>>>> GetCategoryList(CategorySearchModel searchModel)
        {
            var response = new Acknowledgement<JsonResultPaging<List<CategoryResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Category>(true);

                if (searchModel.mode == "ADMIN")
                {
                    predicate = predicate.And(i => (i.CategoryCode != "ALL"));
                }

                if (!string.IsNullOrEmpty(searchModel.searchCode))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchCode.Trim().ToLower());
                    predicate = predicate.And(i => (i.CategoryCode.Trim().ToLower().Contains(searchStringNonUnicode)));
                }

                if (!string.IsNullOrEmpty(searchModel.searchType))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.searchType.Trim().ToLower());
                    predicate = predicate.And(i => (i.CategoryType.Trim().ToLower().Contains(searchStringNonUnicode)));
                }

                var tennantDbList = await _categoryRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<CategoryResponseModel>>(tennantDbList.Data);
                response.Data = new JsonResultPaging<List<CategoryResponseModel>>()
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
                _logger.LogError("GetUserList " + ex.Message);
                return response;
            }
        }
    }
}
