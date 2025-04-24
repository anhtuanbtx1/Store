using System;
using System.Collections.Generic;

namespace Store.Domain.Entity;

public partial class Banner
{
    public int BannerId { get; set; }

    public string? BannerTitle { get; set; }
    public string? BannerSubTitle { get; set; }

    public string? BannerCode { get; set; }

    public string? BannerName { get; set; }

    public string? BannerTypeCode { get; set; }

    public string? BannerTypeName { get; set; }

    public string? BannerImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
