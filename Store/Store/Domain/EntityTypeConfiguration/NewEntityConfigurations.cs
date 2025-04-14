using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entity;

namespace Store.Domain.EntityTypeConfiguration
{
    public class NewEntityConfigurations : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            // Primary key
            builder.HasKey(p => p.news_id);

            // Properties
            builder.Property(p => p.news_thumbnail).HasColumnName("news_thumbnail");
            builder.Property(p => p.news_detail_content).HasColumnName("news_detail_content");
            builder.Property(p => p.news_short_content).HasColumnName("news_short_content");
            builder.Property(p => p.state).HasColumnName("state");

            // Table
            builder.ToTable("News");
        }
    }
}
