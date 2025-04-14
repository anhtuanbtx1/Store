using System;
using System.Collections.Generic;

namespace Store.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryType { get; set; }

    public string? CategoryCode { get; set; }

    public string? CategoryName { get; set; }
}
