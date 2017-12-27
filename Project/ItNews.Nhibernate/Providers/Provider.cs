using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItNews.Nhibernate.Providers
{
    public class Provider<T> : IProvider<T>
        where T : class, IEntity
    {
        protected SessionManager sessionManager;

        public Provider(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public Task Delete(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance?.Id))
                throw new ArgumentNullException("Id");

            return sessionManager.Session.DeleteAsync(instance);
        }

        public Task<T> Get(string id)
        {
            return sessionManager.Session.GetAsync<T>(id);
        }

        public Task<int> GetCount()
        {
            return sessionManager.Session.QueryOver<T>().RowCountAsync();
        }

        public Task<IList<T>> GetList()
        {
            return sessionManager.Session.QueryOver<T>().ListAsync();
        }

        public async Task<T> SaveOrUpdate(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance.Id))
                instance.Id = Guid.NewGuid().ToString();

            await sessionManager.Session.SaveOrUpdateAsync(instance);
            return instance;
        }
    }
}
