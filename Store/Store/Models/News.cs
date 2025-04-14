namespace Store.Models
{
    public partial class News
    {
        public int NewsId { get; set; }

        public string? NewsThumbnail { get; set; }

        public string? NewsShortContent { get; set; }

        public string? NewsDetailContent { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int State { get; set; }
    }
}
