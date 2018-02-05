using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Search
{
    public interface ISearchProvider<T>
        where T : class, IEntity
    {
        T SearchOne(string query);
        IEnumerable<T> SearchMany(string query);
    }
}
