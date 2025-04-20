using AutoMapper;
using LinqKit;
using Store.Common.BaseModels;
using Store.Common.Helper;
using Store.DAL.Interfaces;
using Store.DAL.Services.Interfaces;
using Store.Domain.Entity;
using Store.Models.Request;
using Store.Models.Respone;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public string GetImageFileUrl(string rawUrl)
        {
            var prefixUrl = Configuration.GetSection("FTP:Host").Value;
            return prefixUrl + rawUrl;
        }
        public async Task<Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>> GetNewsList()
        {
            var response = new Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>();
            try
            {
                var predicate = PredicateBuilder.New<News>(i => i.State == true);
                var tennantDbList = await _newRepository.ReadOnlyRespository.GetWithPagingAsync(
                   new PagingParameters(1, 100),
                   predicate
                   );
                var data = _mapper.Map<List<NewsResponseModel>>(tennantDbList.Data);
                data.ForEach(i =>
                {
                    if (!string.IsNullOrWhiteSpace(i.newsThumbnail))
                    {
                        i.newsThumbnail = GetImageFileUrl(i.newsThumbnail);
                    }
                    else
                    {
                        i.newsThumbnail = "../content/images/noImage.png";
                    }
                });
                response.Data = new JsonResultPaging<List<NewsResponseModel>>()
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
                _logger.LogError("GetNewList " + ex.Message);
                return response;
            }
        }

        public async Task<Acknowledgement<NewResponseModel>> GetById(int newId)
        {
            var ack = new Acknowledgement<NewResponseModel>();
            try
            {
                var user = await _newRepository.ReadOnlyRespository.FindAsync(newId);
                if (user == null)
                {
                    ack.IsSuccess = false;
                    ack.AddMessages("Không tìm thấy user");
                    return ack;
                }
                var data = _mapper.Map<NewResponseModel>(user);
                data.newsThumbnail = GetImageFileUrl(data.newsThumbnail);

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

        public async Task<Acknowledgement> Update(NewRequestModel postData)
        {
            var ack = new Acknowledgement();
            var existItem = await _newRepository.Repository.FirstOrDefaultAsync(i => i.NewsId == postData.newsId);
            if (existItem == null)
            {
                ack.AddMessage("Không tìm thấy người dùng");
                ack.IsSuccess = false;
                return ack;
            }
            else
            {
                existItem.State = postData.state;
                existItem.NewsTitle = postData.newsTitle;
                existItem.NewsThumbnail = postData.newsThumbnail;
                existItem.NewsDetailContent = postData.newsDetailContent;
                existItem.UpdatedAt = DateTime.Now;
                existItem.NewsShortContent = postData.newsShortContent;
             
                await ack.TrySaveChangesAsync(res => res.UpdateAsync(existItem), _newRepository.Repository);
            }


            return ack;
        }
    }
}
