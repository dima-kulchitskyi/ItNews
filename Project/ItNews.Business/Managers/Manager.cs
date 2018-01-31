using System.Threading;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public abstract class Manager<T, TProvider>
        where T : IEntity
        where TProvider : IProvider<T>
    {
        protected TProvider provider;

        public Manager(TProvider provider)
        {
            this.provider = provider;
        }

        public Task<T> GetById(string id)
        {
            return provider.Get(id);
        }
    }
}
