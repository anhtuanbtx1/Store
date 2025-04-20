using Store.Common.BaseModels;
using Store.Models.Respone;
using Store.Models.Response;
using Store.Models.Search;

namespace Store.DAL.Services.Interfaces
{
    public interface ITemplateService : IBaseService, IDisposable
    {
        Task<Acknowledgement<JsonResultPaging<List<TemplateResponseModel>>>> GetTemplateList(TemplateSearchModel searchModel);
    }
}
