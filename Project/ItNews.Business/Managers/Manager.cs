using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public class Manager<T>
        where T : IEntity
    {
        protected IProvider<T> provider;

        public Manager(IProvider<T> provider)
        {
            this.provider = provider;
        }

        public Task<int> GetCountAsync()
        {
            return provider.GetCount();
        }

        public Task<T> GetAsync(string id)
        {
            return provider.GetAsync(id);
        }
    }
}
