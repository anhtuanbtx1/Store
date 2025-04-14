using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Entity;

public class News
{
    [Key]
    public int news_id { get; set; }

    public string? news_thumbnail { get; set; }

    public string? news_short_content { get; set; }

    public string? news_detail_content { get; set; }

    public DateTime? created_date { get; set; }

    public DateTime? updated_date { get; set; }

    public bool? state { get; set; }
}
