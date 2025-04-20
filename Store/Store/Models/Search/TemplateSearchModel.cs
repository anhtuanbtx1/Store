using Store.Common.BaseModels;

namespace Store.Models.Search
{
    public class TemplateSearchModel : SearchPagingModel<ProductSearchModel>
    {
        public string? templateCode { get; set; }
    }
}
