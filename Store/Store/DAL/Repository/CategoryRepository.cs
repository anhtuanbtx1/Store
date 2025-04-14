using Store.DAL.Interfaces;
using Store.Domain.DBContexts;
using Store.Domain.Entity;
using System.Data;

namespace Store.DAL.Repository
{
    public class CategoryRepository : RepositoryGenerator<Category>, ICategoryRepository
    {
        public CategoryRepository(SampleDBContext context, SampleReadOnlyDBContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {
            
        }
    }
}
