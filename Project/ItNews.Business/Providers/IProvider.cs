using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IProvider<T> 
        where T : IEntity
    {
        Task<T> GetAsync(string id);
        Task<T> SaveOrUpdateAsync(T instance);
        Task DeleteAsync(T instance);
        Task<IList<T>> GetListAsync();
    }
}
