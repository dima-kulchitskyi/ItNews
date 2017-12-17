using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItNews.Nhibernate.Providers
{
    public class ArticleProvider : NhibernateProvider<Article>, IArticleProvider
    {
        public Task<IList<Article>> GetListSegment(int count, DateTime startDate)
        {
            using (var uow = new UnitOfWork(false))
                return uow.SessionManager.Session.QueryOver<Article>().Where(it => it.Date.CompareTo(startDate) > 0).ListAsync();
        }
    }
}
