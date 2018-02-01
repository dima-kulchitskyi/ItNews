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
        public TProvider Provider { get; protected set; }

        public Manager(IDependencyResolver dependencyResolver)
        {
            var config = dependencyResolver.Resolve<ServerVariables>();
            Provider = dependencyResolver.Resolve<TProvider>(config.DataSourceProviderType);
        }

        public Task<T> GetById(string id)
        {
            return Provider.Get(id);
        }
    }
}
