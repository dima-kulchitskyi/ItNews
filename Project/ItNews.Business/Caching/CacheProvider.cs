using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Caching
{
    public class CacheProvider<T>
        where T : class, IEntity
    {
        public T GetById(string id)
        {
            return MemoryCache.Default.Get(typeof(T).FullName + id) as T;
        }

        public void Save(T entity)
        {
            MemoryCache.Default.Set(typeof(T).FullName + entity.Id, entity, DateTime.Now.AddMinutes(30));
        }
    }
}
