using Store.Domain.Entity;

namespace Store.Models.Response
{
    public class BannerResponseModel : BaseAuditableEntity
    {
        public int bannerId { get; set; }

        public string? bannerTitle { get; set; }

        public string? bannerCode { get; set; }

        public string? bannerName { get; set; }

        public string? bannerTypeCode { get; set; }

        public string? bannerTypeName { get; set; }

        public string? bannerImage { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
