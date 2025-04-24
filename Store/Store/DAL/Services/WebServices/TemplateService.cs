using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Util;
using Store.DAL.Interfaces;
using Store.DAL.Repository;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Respone;
using Store.Models.Response;
using Store.Models.Search;

namespace Store.DAL.Services.WebServices
{
    public class TemplateService : BaseService<TemplateService>, ITemplateService
    {
        private readonly IMapper _mapper;
        private readonly ITemplateRepository _templateRepository;
        public TemplateService(
         ILogger<TemplateService> logger,
         IHttpContextAccessor httpContextAccessor,
         IConfiguration configuration,
         ITemplateRepository templateRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _templateRepository = templateRepository;
        }

        public async Task<Acknowledgement<JsonResultPaging<List<TemplateResponseModel>>>> GetTemplateList(TemplateSearchModel searchModel)
        {
            var response = new Acknowledgement<JsonResultPaging<List<TemplateResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<Template>(true);
                if (!string.IsNullOrEmpty(searchModel.templateCode))
                {
                    var searchStringNonUnicode = Utils.NonUnicode(searchModel.templateCode.Trim().ToLower());
                    predicate = predicate.And(i => (i.TemplateCode.Trim().ToLower().Contains(searchStringNonUnicode)));
                }
                var tennantDbList = await _templateRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<TemplateResponseModel>>(tennantDbList.Data);
                response.Data = new JsonResultPaging<List<TemplateResponseModel>>()
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
                _logger.LogError("GetTemplateList " + ex.Message);
                return response;
            }
        }


    }
}
