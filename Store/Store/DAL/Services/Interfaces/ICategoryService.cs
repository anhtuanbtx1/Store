using Store.Common.BaseModels;
using Store.Models.Respone;
using Store.Models.Search;

namespace Store.DAL.Services.Interfaces
{
    public interface ICategoryService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<CategoryResponseModel>>>> GetCategoryList(CategorySearchModel searchModel);


    }
}
