using Store.DAL.Interfaces;
using Store.Domain.Entity;

namespace Store.DAL.Repository
{
    public class NewRepository : RepositoryGenerator<News>, INewRepository
    {
        public NewRepository(ManagementStoreContext context, ManagementStoreContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {

        }
    }
}
