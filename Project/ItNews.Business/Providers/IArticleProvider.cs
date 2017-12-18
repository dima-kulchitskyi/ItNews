using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItNews.Business.Providers
{
    public interface IArticleProvider : IProvider<Article> 
    {
        Task<IList<Article>> GetListSegmentAsync(int count, DateTime startDate, bool newFirst);
    }
}
