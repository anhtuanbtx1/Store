using Store.Models.Image;

namespace Store.Models.Request
{
    public class BannerRequestModel
    {
        public int bannerId { get; set; }
        public string? bannerName { get; set; }
        public string? bannerTitle { get; set; }
        public string? bannerSubTitle { get; set; }
        public string? bannerImage { get; set; }
        public string? uploadFile { get; set; }
        public List<UploadImageModel>? listUploadFiles { get; set; }
    }
}
