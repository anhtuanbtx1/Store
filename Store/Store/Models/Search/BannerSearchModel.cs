using Store.Common.BaseModels;

namespace Store.Models.Search
{
    public class BannerSearchModel : SearchPagingModel<CategorySearchModel>
    {
        public string? searchType { get; set; }
        public string? searchCode { get; set; }
    }
}
