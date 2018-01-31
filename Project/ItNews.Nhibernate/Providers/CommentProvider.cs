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
        public async Task<IList<Comment>> GetArticleComments(string articleId)
        {
            using (var container = SessionContainer.Open())
            {
                return await container.Session.QueryOver<Comment>().Where(comment => comment.Article.Id == articleId).ListAsync();
            }
        }
    }
}
