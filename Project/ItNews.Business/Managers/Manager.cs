using ItNews.Business.Caching;
using ItNews.Business.Entities;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class Manager<T>
        where T : class, IEntity
    {
        protected IProvider<T> provider;
        protected CacheProvider<T> cacheProvider;

        protected IUnitOfWorkFactory unitOfWorkFactory;

        public Manager(IProvider<T> provider, IUnitOfWorkFactory unitOfWorkFactory, CacheProvider<T> cacheProvider)
        {
            this.provider = provider;
            this.cacheProvider = cacheProvider;

            this.unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}
