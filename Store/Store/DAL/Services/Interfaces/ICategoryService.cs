using Store.Common.BaseModels;
using Store.Models.Respone;

namespace Store.DAL.Services.Interfaces
{
    public interface ICategoryService : IBaseService, IDisposable
    {

        Task<Acknowledgement<JsonResultPaging<List<CategoryResponeModel>>>> GetTenantList();
    }
}
