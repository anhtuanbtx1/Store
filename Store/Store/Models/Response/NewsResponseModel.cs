﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Store.Domain.Entity;

namespace Store.Models.Respone
{
    public class NewsResponseModel : BaseAuditableEntity
    {
        public int newsId { get; set; }
        public string newsTitle { get; set; }
        public string newsThumbnail { get; set; }
        public string newsShortContent { get; set; }
        public bool state { get; set; }
        public DateTime updatedAt { get; set; }
    }

    public class NewResponseModel
    {
        public int newsId { get; set; }
        public string newsTitle { get; set; }
        public string newsThumbnail { get; set; }
        public string newsShortContent { get; set; }
        public string newsDetailContent { get; set; }
        public bool state { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
