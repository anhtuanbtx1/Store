using Store.Common.BaseModels;
using Store.Models.Respone;

namespace Store.DAL.Services.Interfaces
{

    public interface INewService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<NewsResponseModel>>>> GetNewsList();
        Task<Acknowledgement> Update(NewsResponseModel postData);
        Task<Acknowledgement<NewResponseModel>> GetUserById(int newId);

    }
}
