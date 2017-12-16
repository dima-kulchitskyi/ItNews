using ItNews.Business.Entities.Interfaces;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Nhibernate
{
    public class Provider<T> : IProvider<T> 
        where T : IEntity
    {
        public T Delete(T instance)
        {
            throw new NotImplementedException();
        }

        public T GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Save(T instance)
        {
            throw new NotImplementedException();
        }

        public T Update(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
