using Store.Common.BaseModels;
using Store.Models.Respone;

namespace Store.DAL.Services.Interfaces
{
    public interface IProductService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>> GetProductList();
        Task<Acknowledgement> Update(NewsResponseModel postData);
        Task<Acknowledgement<NewReponseModel>> GetUserById(int newId);
    }
}
