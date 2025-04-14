using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;

namespace Store.Domain.EntityTypeConfiguration
{
    public class CategoryEntityConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Primary key
            builder.HasKey(p => p.category_id);

            // Properties
            builder.Property(p => p.category_code).HasColumnName("category_code");
            builder.Property(p => p.category_type).HasColumnName("category_type");
            builder.Property(p => p.category_name).HasColumnName("category_name");

            // Table
            builder.ToTable("Category");
        }
    }
}
