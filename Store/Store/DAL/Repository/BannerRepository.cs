using Store.DAL.Interfaces;
using Store.Domain.Entity;

namespace Store.DAL.Repository
{
    public class BannerRepository : RepositoryGenerator<Banner>, IBannerRepository
    {
        public BannerRepository(ManagementStoreContext context, ManagementStoreContext readOnlyDBContext) : base(context, readOnlyDBContext)
        {

        }
    }
}
