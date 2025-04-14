using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Store.Domain.Entity
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }

        public string? category_type { get; set; }

        public string? category_code { get; set; }

        public string? category_name { get; set; }
    }
}
