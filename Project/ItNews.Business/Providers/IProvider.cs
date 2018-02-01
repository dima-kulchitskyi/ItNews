using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IProvider<T> 
        where T : IEntity
    {
        IUnitOfWork GetUnitOfWork();
        Task<T> Get(string id);
        Task<T> SaveOrUpdate(T instance);
        Task DeleteAsync(T instance);
        Task<IList<T>> GetList();
        Task<int> GetCount();
    }
}
