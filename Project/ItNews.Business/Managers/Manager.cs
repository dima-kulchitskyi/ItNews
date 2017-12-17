using ItNews.Business.Entities;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public abstract class Manager<TProvider, T>
        where TProvider : IProvider<T> 
        where T : IEntity
    {

    }
}
