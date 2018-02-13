using ItNews.Business;
using ItNews.Business.Entities;
using ItNews.Business.Providers;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ItNews.Nhibernate;
using NHibernate.Criterion;

namespace ItNews.Nhibernate.Providers
{
    public class Provider<T> : IProvider<T>
        where T : class, IEntity
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }

        public async Task Delete(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance?.Id))
                throw new ArgumentNullException("Id");

            using (var uow = GetUnitOfWork())
            {
                uow.BeginTransaction();

                await ProviderHelper.GetSession(s =>
                   s.DeleteAsync(instance));

                await uow.Commit();
            }
        }

        public Task<T> Get(string id)
        {
            return ProviderHelper.Invoke(s =>
               s.GetAsync<T>(id));
        }

        public Task<IList<T>> Get(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (ids.Count() == 0)
                throw new ArgumentException($"{nameof(ids)} is empty");

            return ProviderHelper.Invoke(async s =>
           {
               if (ids.Count() == 1)
                   return new List<T> { await s.GetAsync<T>(ids.First()) };

               return await s.QueryOver<T>().WhereRestrictionOn(it => it.Id).IsIn(ids.ToArray()).ListAsync();
           });
        }

        public Task<int> GetCount()
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<T>().RowCountAsync());
        }

        public Task<IList<T>> GetList()
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<T>().ListAsync());
        }

        public async Task<T> SaveOrUpdate(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance.Id))
                instance.Id = Guid.NewGuid().ToString();

            using (var uow = GetUnitOfWork())
            {
                uow.BeginTransaction();

                await ProviderHelper.GetSession(async s =>
                    await s.SaveOrUpdateAsync(instance));

                await uow.Commit();
            }

            return instance;
        }
    }
}
