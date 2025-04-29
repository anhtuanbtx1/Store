using Store.Models.Image;

namespace Store.Models.Request
{
    public class NewRequestModel
    {
        public int newsId { get; set; }
        public string newsTitle { get; set; }
        public string newsShortContent { get; set; }
        public string newsThumbnail { get; set; }
        public string newsDetailContent { get; set; }
        public bool state { get; set; }
        public string? uploadFile { get; set; }
        public List<UploadImageModel>? listUploadFiles { get; set; }
    }
}
