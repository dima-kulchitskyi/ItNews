using ItNews.Business.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IProvider<T> 
        where T : IEntity
    {
        T GetById(Guid id);
        T Save(T instance);
        T Update(T instance);
        T Delete(T instance);
    }
}
