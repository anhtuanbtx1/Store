namespace Store.Models.Image
{
    public class UploadImageModel
    {
        public List<IFormFile> Image { get; set; }
    }
    public class ImageModel
    {
        public int imageId { get; set; }
        public int? warantyId { get; set; }
        public string link { get; set; }
        public string linkName { get; set; }
        public string type { get; set; }
        public DateTime createdAt { get; set; }
    }
}
