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
        public async Task<IList<Article>> GetListSegment(int count, DateTime startDate, bool newFirst)
        {
            using (var container = SessionContainer.Open())
            {
                var criteria = container.Session.CreateCriteria<Article>();

                if (newFirst)
                    criteria.AddOrder(Order.Desc(nameof(Article.Date)))
                            .Add(Restrictions.Lt(nameof(Article.Date), startDate));
                else
                    criteria.AddOrder(Order.Asc(nameof(Article.Date)))
                            .Add(Restrictions.Gt(nameof(Article.Date), startDate));

                criteria.SetMaxResults(count);

                return await criteria.ListAsync<Article>();
            }
        }

        public async Task<IList<Article>> GetPage(int count, int pageNumber, bool newFirst)
        {
            using (var container = SessionContainer.Open())
            {
                var criteria = container.Session.CreateCriteria<Article>();

                if (newFirst)
                    criteria.AddOrder(Order.Desc(nameof(Article.Date)));
                else
                    criteria.AddOrder(Order.Asc(nameof(Article.Date)));

                criteria.SetFirstResult(count * pageNumber);
                criteria.SetMaxResults(count);

                return await criteria.ListAsync<Article>();
            }
        }
    }
}