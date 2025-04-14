using Store.DAL.Interfaces;
using Store.Domain.DBContexts;
using Store.Domain.Entity;

namespace Store.DAL.Repository
{
    public class NewRepository : RepositoryGenerator<News>, INewRepository
    {
        public NewRepository(SampleDBContext context, SampleReadOnlyDBContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {

        }
    }
}
