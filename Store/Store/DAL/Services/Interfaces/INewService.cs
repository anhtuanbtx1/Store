using Store.Common.BaseModels;
using Store.Models.Request;
using Store.Models.Respone;

namespace Store.DAL.Services.Interfaces
{

    public interface INewService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>> GetNewsList();
        Task<Acknowledgement> Update(NewRequestModel postData);
        Task<Acknowledgement<NewResponseModel>> GetById(int newId);

    }
}
