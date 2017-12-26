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
        public ArticleProvider(SessionManager sessionManager) : base(sessionManager)
        {
        }

        public Task<IList<Article>> GetListSegmentAsync(int count, DateTime startDate, bool newFirst)
        {
            var criteria = sessionManager.Session.CreateCriteria<Article>();

            if (newFirst)
                criteria.AddOrder(Order.Desc("Date"))
                        .Add(Restrictions.Lt("Date", startDate));
            else
                criteria.AddOrder(Order.Asc("Date"))
                        .Add(Restrictions.Gt("Date", startDate));

            return criteria.SetMaxResults(count).ListAsync<Article>();
        }

        public Task<IList<Article>> GetPage(int count, int pageNumber, bool newFirst)
        {
            var criteria = sessionManager.Session.CreateCriteria<Article>();

            if (newFirst)
                criteria.AddOrder(Order.Desc("Date"));
            else
                criteria.AddOrder(Order.Asc("Date"));
            criteria.SetFirstResult(count * (pageNumber - 1) + 1);
            criteria.SetMaxResults(count * pageNumber);
            return criteria.SetMaxResults(count).ListAsync<Article>();

        }
    }
}