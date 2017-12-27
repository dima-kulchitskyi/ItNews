using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class ArticleManager : Manager<Article>
    {
        private IArticleProvider articleProvider;

        public ArticleManager(IArticleProvider provider) : base(provider)
        {
            articleProvider = provider;
        }

        public Task<IList<Article>> GetListSegmentAsync(int count, DateTime startDate, bool newFirst)
        {
            return articleProvider.GetListSegmentAsync(count, startDate, newFirst);
        }

        public Task<IList<Article>> GetPagesAsync(int count, int pageNumber, bool newFirst)
        {
            return articleProvider.GetPage(count, pageNumber, newFirst);
        }
    }
}
