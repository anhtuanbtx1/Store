using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Models.Image;

namespace Store.Models.Request
{
    public class ProductRequestModel
    {
        public int productId { get; set; }

        public string productName { get; set; } = null!;

        public string? productStatusCode { get; set; }

        public string? productStatusName { get; set; }

        public string? productSpaceCode { get; set; }

        public string? productSpaceName { get; set; }

        public string? productSeriesCode { get; set; }

        public string? productSeriesName { get; set; }

        public string? productColorCode { get; set; }

        public string? productColorName { get; set; }

        public string? productPrice { get; set; }

        public string? productPriceSale { get; set; }

        public List<string>? listImage { get; set; }
        public List<string>? uploadFiles { get; set; }
        public List<UploadImageModel>? listUploadFiles { get; set; }

        public string? productDetail { get; set; }
        public string? productShortDetail { get; set; }
    }
}
