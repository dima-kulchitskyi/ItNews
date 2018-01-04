using ItNews.Business.Entities;
using ItNews.Business.Providers;

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
    }
}
