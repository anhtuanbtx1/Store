using Store.DAL.Interfaces;
using Store.Domain.Entity;
using System.Data;

namespace Store.DAL.Repository
{
    public class CategoryRepository : RepositoryGenerator<Category>, ICategoryRepository
    {
        public CategoryRepository(ManagementStoreContext context, ManagementStoreContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {
            
        }
    }
}
