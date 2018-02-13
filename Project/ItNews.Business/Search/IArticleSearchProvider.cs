using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItNews.Business.Search
{
    public interface IArticleSearchProvider : ISearchProvider<Article>
    {
    }
}
