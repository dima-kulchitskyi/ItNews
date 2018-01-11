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

            if (sessionManager.GetExistingOrOpenSession().Transaction?.IsActive != true)
                throw new InvalidOperationException("Transaction required");

            return sessionManager.GetExistingOrOpenSession().DeleteAsync(instance);
        }

        public Task<T> Get(string id)
        {
            return sessionManager.GetExistingOrOpenSession().GetAsync<T>(id);
        }

        public Task<int> GetCount()
        {
            return sessionManager.GetExistingOrOpenSession().QueryOver<T>().RowCountAsync();
        }

        public Task<IList<T>> GetList()
        {
            return sessionManager.GetExistingOrOpenSession().QueryOver<T>().ListAsync();
        }

        public async Task<T> SaveOrUpdate(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (sessionManager.GetExistingOrOpenSession().Transaction?.IsActive != true)
                throw new InvalidOperationException("Transaction required");

            if (string.IsNullOrEmpty(instance.Id))
                instance.Id = Guid.NewGuid().ToString();
            
            await sessionManager.GetExistingOrOpenSession().SaveOrUpdateAsync(instance);
            return instance;
        }
    }
}
