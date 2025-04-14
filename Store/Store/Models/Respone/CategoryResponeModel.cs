using Store.Domain.Entity;

namespace Store.Models.Respone
{
    public class CategoryResponeModel : BaseAuditableEntity
    {
        public int category_id { get; set; }
        public string category_type { get; set; }
        public string category_code { get; set; }
        public string category_name { get; set; }
    }
}
