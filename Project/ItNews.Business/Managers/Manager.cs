using System.Threading;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Threading.Tasks;
using ItNews.Business.Caching;

namespace ItNews.Business.Managers
{
    public abstract class Manager<T, TProvider, TCacheProvider>
        where T : class, IEntity
        where TProvider : IProvider<T>
        where TCacheProvider : CacheProvider<T>
    {
        protected TProvider provider;
        protected TCacheProvider cacheProvider;

        public Manager(IDependencyResolver dependencyResolver)
        {
            provider = dependencyResolver.Resolve<TProvider>(ApplicationVariables.DataSourceProviderType);
            cacheProvider = dependencyResolver.Resolve<TCacheProvider>();
        }

        public Task<T> GetById(string id, bool useCache = true)
        {
            if (!useCache)
                return provider.Get(id);

            return cacheProvider.GetById(id, () => this.GetById(id, false));
        }
    }
}
