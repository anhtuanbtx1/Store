using Store.Domain.DBContexts;
using System.Security.Claims;

namespace Store.DAL.Services.WebServices
{
    public abstract class BaseService<T> : IDisposable
    {
        public readonly ILogger<T> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;
      

        public BaseService(
            ILogger<T> logger,
            IConfiguration configuration,
          
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }
        private SampleDBContext _DbContext;
        public SampleDBContext DbContext
        {
            get
            {

                if (_DbContext == null)
                {
                    _DbContext = new SampleDBContext();
                }
                return _DbContext;
            }
        }
        public void Dispose()
        {
            DbContext.Dispose();
        }
        ~BaseService()
        {
            Dispose();
        }
    }
}
