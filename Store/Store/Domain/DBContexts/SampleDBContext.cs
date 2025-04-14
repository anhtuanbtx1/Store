using Microsoft.EntityFrameworkCore;
using Store.Domain.Entity;
using Store.Domain.EntityTypeConfiguration;
using System;

namespace Store.Domain.DBContexts
{
    public class SampleReadOnlyDBContext : SampleDBContext
    {
        public SampleReadOnlyDBContext() : base()
        {
        }
        public SampleReadOnlyDBContext(DbContextOptions<SampleDBContext> options)
           : base(options)
        {
            
        }
        public override int SaveChanges()
        {
            throw new NotSupportedException();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
    }

    public class SampleDBContext : DbContext
    {
        public SampleDBContext(DbContextOptions<SampleDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityConfigurations());
            base.OnModelCreating(modelBuilder);
            // Cấu hình thêm cho model nếu cần
        }
        public SampleDBContext() : base()
        {
            
        }
        public virtual DbSet<Category> Categorys { get; set; }
    }
}
