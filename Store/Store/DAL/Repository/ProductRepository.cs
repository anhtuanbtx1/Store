using Store.DAL.Interfaces;
using Store.Domain.Entity;

namespace Store.DAL.Repository
{
    public class ProductRepository : RepositoryGenerator<Product>, IProductRepository
    {
        public ProductRepository(ManagementStoreContext context, ManagementStoreContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {

        }
    }
}
