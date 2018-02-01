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
        ApplicationVariables applicationVariables;

        protected readonly string keyPrefix;

        public CacheProvider(IDependencyResolver dependencyResolver)
        {
            applicationVariables = dependencyResolver.Resolve<ApplicationVariables>();
            keyPrefix = applicationVariables.DataSourceProviderType + typeof(T).FullName;
        }
        public T GetById(string id)
        {
            return MemoryCache.Default.Get(keyPrefix + id) as T;
        }

        public T Save(T entity)
        {
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.Id))
                    throw new ArgumentException("Entity id required");

                MemoryCache.Default.Set(keyPrefix + entity.Id, entity, DateTime.Now.AddMinutes(30));
            }
            return entity;
        }

        public void Clear(string id)
        {
            var key = keyPrefix + id;
            if (MemoryCache.Default.Contains(key))
                MemoryCache.Default.Remove(key);
        }
    }
}