using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Respone;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace Store.DAL.Services.WebServices
{
    public class NewService : BaseService<NewService>, INewService
    {
        private readonly IMapper _mapper;
        private readonly INewRepository _newRepository;
        public NewService(
         ILogger<NewService> logger,
         IHttpContextAccessor httpContextAccessor,
         IConfiguration configuration,
         INewRepository newRepository,
         IMapper mapper
         ) : base(logger, configuration, httpContextAccessor)
        {
            _mapper = mapper;
            _newRepository = newRepository;
        }

        public async Task<Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>> GetTenantList()
        {
            var response = new Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<News>(true);
                var tennantDbList = await _newRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<NewsResponseModel>>(tennantDbList.Data);
                response.Data = new JsonResultPaging<List<NewsResponseModel>>()
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


    

        public async Task<Acknowledgement> Update(NewsResponseModel postData)
        {
            var ack = new Acknowledgement();
            var existItem = await _newRepository.Repository.FirstOrDefaultAsync(i => i.news_id == postData.newsId);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy người dùng");
                ack.IsSuccess = false;
                return ack;
            }
            else
            {
                existItem.state = postData.state;
                existItem.news_thumbnail = postData.newsThumbnail;
                await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _newRepository.Repository);
            }


            return ack;
        }
    }
}
