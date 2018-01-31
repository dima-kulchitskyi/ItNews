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
       
        public async Task<int> GetArticleCommentsCount(string articleId)
        {
            using (var sessionContainer = SessionContainer.Open())
            {
                return await sessionContainer.Session.QueryOver<Comment>().Where(comment => comment.Article.Id == articleId).RowCountAsync();
            }
        }

        public async Task<IList<Comment>> GetArticleCommentsPage(string articleId, int itemsCount, int commentPage)
        {
            using (var container = SessionContainer.Open())
            {
                return await container.Session.QueryOver<Comment>().Where(comment => comment.Article.Id == articleId).OrderBy(m => m.Date).Desc.Skip(commentPage).Take(itemsCount).ListAsync();
            }
        }
    }
}
