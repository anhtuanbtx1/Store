using System;
using System.Collections.Generic;

namespace Store.Domain.Entity;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductStatus { get; set; }

    public string? ProductSpace { get; set; }

    public string? ProductSeries { get; set; }

    public string? ProductColor { get; set; }

    public string? ProductPrice { get; set; }

    public string? ProductPriceSale { get; set; }

    public string? ProductImage { get; set; }

    public string? ProductDetail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
