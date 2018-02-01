using ItNews.Business.Entities;
using ItNews.Business.Providers;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItNews.Business;

namespace ItNews.Nhibernate.Providers
{
    public class ArticleProvider : Provider<Article>, IArticleProvider
    {
        public async Task<IList<Article>> GetListSegment(int count, DateTime startDate)
        {
            using (var container = SessionContainer.Open())
            {
                var criteria = container.Session.CreateCriteria<Article>()
                        .AddOrder(Order.Desc(nameof(Article.Date)))
                        .Add(Restrictions.Lt(nameof(Article.Date), startDate))
                        .SetMaxResults(count);

                return await criteria.ListAsync<Article>();
            }
        }

        public async Task<IList<Article>> GetPage(int count, int pageNumber)
        {
            using (var container = SessionContainer.Open())
            {
                var criteria = container.Session.CreateCriteria<Article>()
                        .AddOrder(Order.Desc(nameof(Article.Date)))
                        .SetFirstResult(count * pageNumber)
                        .SetMaxResults(count);

                return await criteria.ListAsync<Article>();
            }
        }
    }
}