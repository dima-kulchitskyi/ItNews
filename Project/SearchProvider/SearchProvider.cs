using ItNews.Business.Entities;
using ItNews.Business.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchProvider
{
    public class SearchProvider<T> : ISearchProvider<T>
        where T : class, IEntity
    {
        public IEnumerable<T> SearchMany(string query)
        {
            throw new NotImplementedException();
        }

        public T SearchOne(string query)
        {
            throw new NotImplementedException();
        }
    }
}
