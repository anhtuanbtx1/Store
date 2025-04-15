using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;

namespace Store.Models;

public partial class ManagementStoreContext : DbContext
{
    public ManagementStoreContext()
    {
    }

    public ManagementStoreContext(DbContextOptions<ManagementStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.0.0.11;Database=ManagementStore;User Id=sa;Password=Ots@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.NewsDetailContent).HasColumnName("news_detail_content");
            entity.Property(e => e.NewsShortContent)
                .HasMaxLength(255)
                .HasColumnName("news_short_content");
            entity.Property(e => e.NewsThumbnail)
                .HasMaxLength(255)
                .HasColumnName("news_thumbnail");
            entity.Property(e => e.State)
                .HasDefaultValue(false)
                .HasColumnName("state");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
