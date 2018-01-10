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
        protected SessionContainerFactory sessionFactory;

        public Provider(SessionContainerFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public async Task Delete(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance?.Id))
                throw new ArgumentNullException("Id");

            using (var sessionContainer = sessionFactory.CreateSessionContainer())
                await sessionContainer.Session.DeleteAsync(instance);
        }

        public async Task<T> Get(string id)
        {
            using (var sessionContainer = sessionFactory.CreateSessionContainer())
                return await sessionContainer.Session.GetAsync<T>(id);
        }

        public async Task<int> GetCount()
        {
            using (var sessionContainer = sessionFactory.CreateSessionContainer())
                return await sessionContainer.Session.QueryOver<T>().RowCountAsync();
        }

        public async Task<IList<T>> GetList()
        {
            using (var sessionContainer = sessionFactory.CreateSessionContainer())
                return await sessionContainer.Session.QueryOver<T>().ListAsync();
        }

        public async Task<T> SaveOrUpdate(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance.Id))
                instance.Id = Guid.NewGuid().ToString();

            using (var sessionContainer = sessionFactory.CreateSessionContainer())
                await sessionContainer.Session.SaveOrUpdateAsync(instance);

            return instance;
        }
    }
}
