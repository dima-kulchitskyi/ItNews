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

        public Manager(TProvider provider, TCacheProvider cacheProvider)
        {
            this.provider = provider;
            this.cacheProvider = cacheProvider;
        }

        public async Task<T> GetById(string id)
        {
            return cacheProvider.GetById(id) ?? cacheProvider.Save(await provider.Get(id));
        }
    }
}
