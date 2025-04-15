using Store.Common.BaseModels;
using Store.Models.Request;
using Store.Models.Respone;

namespace Store.DAL.Services.Interfaces
{
    public interface IProductService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>> GetProductList();
        Task<Acknowledgement> CreateOrUpdate(ProductRequestModel postData);
        Task<Acknowledgement<ProductResponseModel>> GetById(int productId);
    }
}
