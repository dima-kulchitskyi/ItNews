using ItNews.Business.Entities;
using ItNews.Business.Providers;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItNews.Business;
using ItNews.Nhibernate;

namespace ItNews.Nhibernate.Providers
{
    public class ArticleProvider : Provider<Article>, IArticleProvider
    {
        public Task<IList<Article>> GetListSegment(int count, DateTime startDate)
        {
            return ProviderHelper.Invoke(s =>
            {
                var criteria = s.CreateCriteria<Article>()
                       .AddOrder(Order.Desc(nameof(Article.Date)))
                       .Add(Restrictions.Lt(nameof(Article.Date), startDate))
                       .SetMaxResults(count);

                return criteria.ListAsync<Article>();
            });

        }

        public Task<IList<Article>> GetPage(int count, int pageNumber)
        {
            return ProviderHelper.Invoke(s =>
            {
                var criteria = s.CreateCriteria<Article>()
                        .AddOrder(Order.Desc(nameof(Article.Date)))
                        .SetFirstResult(count * pageNumber)
                        .SetMaxResults(count);

                return criteria.ListAsync<Article>();
            });
        }
    }
}