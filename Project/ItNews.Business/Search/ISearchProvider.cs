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
        T SearchOne(string query, string searchField = "");
        IEnumerable<T> Search(string query, int maxResults = 0, string searchField = "");
        void AddOrUpdate(T entity);
        void AddOrUpdate(IEnumerable<T> entities);
        void Clear(string id);
        void Clear(IEnumerable<string> ids);
    }
}
