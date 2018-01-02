using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System.Threading.Tasks;

namespace ItNews.Business.Managers
{
    public class Manager<T>
        where T : IEntity
    {
        protected IProvider<T> provider;

        protected IUnitOfWorkFactory unitOfWorkFactory;

        public Manager(IProvider<T> provider, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.provider = provider;
            this.unitOfWorkFactory = unitOfWorkFactory;
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
