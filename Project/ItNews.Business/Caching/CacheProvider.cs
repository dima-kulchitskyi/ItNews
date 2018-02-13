using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Caching
{
    public class CacheProvider<T>
        where T : class, IEntity
    {
        protected readonly string keyPrefix;
        protected readonly int cacheDuration;

        public CacheProvider(IDependencyResolver dependencyResolver)
        {
            cacheDuration = int.Parse(ConfigurationManager.AppSettings["CacheDuration"]);
          
            keyPrefix = ApplicationVariables.DataSourceProviderType + typeof(T).FullName;
        }

        public T GetById(string id)
        {
            return (T)MemoryCache.Default.Get(keyPrefix + id);
        }

        public virtual void Save(T entity)
        {
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.Id))
                    throw new ArgumentException("Entity id required");

                MemoryCache.Default.Set(keyPrefix + entity.Id, entity, DateTime.Now.AddMinutes(cacheDuration));
            }
        }

        public virtual void Clear(string id)
        {
            var key = keyPrefix + id;
            if (MemoryCache.Default.Contains(key))
                MemoryCache.Default.Remove(key);
        }
    }
}