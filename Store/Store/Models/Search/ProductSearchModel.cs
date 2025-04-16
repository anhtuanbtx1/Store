using Store.Common.BaseModels;

namespace Store.Models.Search
{
    public class ProductSearchModel : SearchPagingModel<ProductSearchModel>
    {
        public string? searchString { get; set; }
        public string? searchStringCode { get; set; }
    }
}
