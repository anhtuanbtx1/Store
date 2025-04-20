using Store.Common.BaseModels;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Search;

namespace Store.DAL.Services.Interfaces
{
    public interface IProductService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<ProductResponseModel>>>> GetProductList(ProductSearchModel searchModel);
        Task<Acknowledgement> CreateOrUpdate(ProductRequestModel postData);
        Task<Acknowledgement<ProductResponseModel>> GetById(int productId);
        Task<Acknowledgement> DeleteById(int userId);
    }
}
