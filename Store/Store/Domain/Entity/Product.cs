using System;
using System.Collections.Generic;

namespace Store.Domain.Entity;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductStatusCode { get; set; }

    public string? ProductStatusName { get; set; }

    public string? ProductSpaceCode { get; set; }

    public string? ProductSpaceName { get; set; }

    public string? ProductSeriesCode { get; set; }

    public string? ProductSeriesName { get; set; }

    public string? ProductColorCode { get; set; }

    public string? ProductColorName { get; set; }

    public string? ProductPrice { get; set; }

    public string? ProductPriceSale { get; set; }

    public string? ProductImage { get; set; }

    public string? ProductDetail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
