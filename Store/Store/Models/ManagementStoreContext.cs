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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
