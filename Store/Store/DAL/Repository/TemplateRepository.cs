using Store.DAL.Interfaces;
using Store.Domain.Entity;

namespace Store.DAL.Repository
{
    public class TemplateRepository : RepositoryGenerator<Template>, ITemplateRepository
    {
        public TemplateRepository(ManagementStoreContext context, ManagementStoreContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {

        }
    }
}
