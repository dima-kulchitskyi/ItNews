using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IArticleProvider : IProvider<Article> 
    {
        Task<IList<Article>> GetListSegment(int count, DateTime startDate);
        Task<IList<Article>> GetPage(int count, int pageNumber);
    }
}
