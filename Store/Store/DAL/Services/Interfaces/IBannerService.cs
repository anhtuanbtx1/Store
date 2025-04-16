using Store.Common.BaseModels;
using Store.Models.Request;
using Store.Models.Respone;
using Store.Models.Response;
using Store.Models.Search;

namespace Store.DAL.Services.Interfaces
{
    public interface IBannerService : IBaseService, IDisposable
    {
        Task<Acknowledgement> FindById(int bannerId);
        Task<Acknowledgement<JsonResultPaging<List<BannerResponseModel>>>> GetBannerList(BannerSearchModel searchModel);
        Task<Acknowledgement> Update(BannerRequestModel postData);
    }
}
