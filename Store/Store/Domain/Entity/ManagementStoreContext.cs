using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Store.Domain.Entity;

public partial class ManagementStoreContext : DbContext
{
    public ManagementStoreContext()
    {
    }

    public ManagementStoreContext(DbContextOptions<ManagementStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banner__10373C34544EB343");

            entity.ToTable("Banner");

            entity.Property(e => e.BannerId).HasColumnName("banner_id");
            entity.Property(e => e.BannerCode)
                .HasMaxLength(100)
                .HasColumnName("banner_code");
            entity.Property(e => e.BannerImage)
                .HasMaxLength(255)
                .HasColumnName("banner_image");
            entity.Property(e => e.BannerName)
                .HasMaxLength(255)
                .HasColumnName("banner_name");
            entity.Property(e => e.BannerTitle)
                .HasMaxLength(255)
                .HasColumnName("banner_title");
            entity.Property(e => e.BannerSubTitle)
                .HasMaxLength(255)
                .HasColumnName("banner_sub_title");
            entity.Property(e => e.BannerTypeCode)
                .HasMaxLength(100)
                .HasColumnName("banner_type_code");
            entity.Property(e => e.BannerTypeName)
                .HasMaxLength(255)
                .HasColumnName("banner_type_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__D54EE9B4A7FB52D5");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryCode)
                .HasMaxLength(100)
                .HasColumnName("category_code");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(256)
                .HasColumnName("category_name");
            entity.Property(e => e.CategoryType)
                .HasMaxLength(255)
                .HasColumnName("category_type");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__4C27CCD8FF97DB3A");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.NewsDetailContent).HasColumnName("news_detail_content");
            entity.Property(e => e.NewsShortContent)
                .HasMaxLength(255)
                .HasColumnName("news_short_content");
            entity.Property(e => e.NewsTitle)
               .HasMaxLength(255)
               .HasColumnName("news_title");


            entity.Property(e => e.NewsThumbnail)
                .HasMaxLength(255)
                .HasColumnName("news_thumbnail");
            entity.Property(e => e.State)
                .HasDefaultValue(false)
                .HasColumnName("state");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__47027DF59DCAA4BB");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductColorCode)
                .HasMaxLength(255)
                .HasColumnName("product_color_code");
            entity.Property(e => e.ProductColorName)
                .HasMaxLength(255)
                .HasColumnName("product_color_name");
            entity.Property(e => e.ProductDetail).HasColumnName("product_detail");
            entity.Property(e => e.ProductShortDetail).HasColumnName("product_short_detail");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .HasColumnName("product_image");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPrice)
                .HasMaxLength(255)
                .HasColumnName("product_price");
            entity.Property(e => e.ProductPriceSale)
                .HasMaxLength(255)
                .HasColumnName("product_price_sale");
            entity.Property(e => e.ProductSeriesCode)
                .HasMaxLength(100)
                .HasColumnName("product_series_code");
            entity.Property(e => e.ProductSeriesName)
                .HasMaxLength(255)
                .HasColumnName("product_series_name");
            entity.Property(e => e.ProductSpaceCode)
                .HasMaxLength(100)
                .HasColumnName("product_space_code");
            entity.Property(e => e.ProductSpaceName)
                .HasMaxLength(255)
                .HasColumnName("product_space_name");
            entity.Property(e => e.ProductStatusCode)
                .HasMaxLength(100)
                .HasColumnName("product_status_code");
            entity.Property(e => e.ProductStatusName)
                .HasMaxLength(255)
                .HasColumnName("product_status_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId).HasName("PK__Template__BE44E079FC1679F7");

            entity.ToTable("Template");

            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.TemplateDetailContent).HasColumnName("template_detail_content");
            entity.Property(e => e.TemplateCode)
                .HasMaxLength(100)
                .HasColumnName("template_code");
            entity.Property(e => e.TemplateName)
                .HasMaxLength(255)
                .HasColumnName("template_name");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
