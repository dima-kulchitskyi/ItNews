using ItNews.Business.Entities;
using ItNews.Business.Providers;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItNews.Business;

namespace ItNews.Nhibernate.Providers
{
    public class CommentProvider : Provider<Comment>, ICommentProvider
    {
        public CommentProvider(SessionManager sessionManager) : base(sessionManager)
        {
        }

        public Task<IList<Comment>> GetArticleComments(string articleId)
        {
            var criteria = sessionManager.GetExistingOrOpenSession().CreateCriteria<Comment>();
            criteria.Add(Restrictions.Eq("Article", articleId));
            return criteria.ListAsync<Comment>();
        }
    }
}
