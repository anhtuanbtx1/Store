using Store.Domain.Entity;

namespace Store.Models.Respone
{
    public class CategoryResponseModel : BaseAuditableEntity
    {
        public int CategoryId { get; set; }

        public string? CategoryType { get; set; }

        public string? CategoryCode { get; set; }

        public string? CategoryName { get; set; }
    }
}
