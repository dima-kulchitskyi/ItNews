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
    public class NhibernateProvider<T> : IProvider<T>
        where T : class, IEntity
    {
        public Task DeleteAsync(T instance)
        {
            if(instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance?.Id))
                throw new ArgumentNullException("Id");

            using (var uow = new UnitOfWork(true))
                return uow.SessionManager.Session.DeleteAsync(instance);
        }

        public Task<T> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetListAsync()
        {
            throw new NotImplementedException();
        }

       

        public async Task<T> SaveOrUpdateAsync(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            using (var uow = new UnitOfWork(true))
            {
                if (string.IsNullOrEmpty(instance.Id))
                {
                    instance.Id = Guid.NewGuid().ToString();
                    return (T)await uow.SessionManager.Session.SaveAsync(instance);
                }
                await uow.SessionManager.Session.UpdateAsync(instance);
                return instance;
            }
        }



    }
}
