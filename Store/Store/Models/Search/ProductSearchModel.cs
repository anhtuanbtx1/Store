using Store.Common.BaseModels;

namespace Store.Models.Search
{
    public class ProductSearchModel : SearchPagingModel<ProductSearchModel>
    {
        public string? searchString { get; set; }
        public string? seriCode { get; set; }
        public string? colorCode { get; set; }
        public string? spaceCode { get; set; }
        public List<string>? statusCodes { get; set; }
    }
}
