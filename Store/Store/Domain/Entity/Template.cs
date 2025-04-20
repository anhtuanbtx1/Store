using System;
using System.Collections.Generic;

namespace Store.Domain.Entity;

public partial class Template
{
    public int TemplateId { get; set; }

    public string? TemplateCode { get; set; }

    public string? TemplateName { get; set; }

    public string? TemplateDetailContent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
