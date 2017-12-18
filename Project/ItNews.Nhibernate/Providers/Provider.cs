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
        protected UnitOfWork unitOfWork;

        public Provider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = (UnitOfWork)unitOfWork;
        }

        public Task DeleteAsync(T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (string.IsNullOrEmpty(instance?.Id))
                throw new ArgumentNullException("Id");

            unitOfWork.BeginTransaction();
            return unitOfWork.SessionManager.Session.DeleteAsync(instance);
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

            unitOfWork.BeginTransaction();
            if (string.IsNullOrEmpty(instance.Id))
            {
                instance.Id = Guid.NewGuid().ToString();
                return (T)await unitOfWork.SessionManager.Session.SaveAsync(instance);
            }
            await unitOfWork.SessionManager.Session.UpdateAsync(instance);
            return instance;

        }



    }
}
