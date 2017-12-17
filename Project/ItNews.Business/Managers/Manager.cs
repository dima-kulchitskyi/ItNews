using ItNews.Business.Entities;
using ItNews.Business.Providers;

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
    }
}
