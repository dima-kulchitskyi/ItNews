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

        public Task<int> GetArticleCommentsCountAsync(string articleId)
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<Comment>().Where(comment => comment.Article.Id == articleId).RowCountAsync());
        }

        public Task<IList<Comment>> GetArticleCommentsPageAsync(string articleId, int itemsCount, int commentPage)
        {
            return ProviderHelper.Invoke(s =>
               s.QueryOver<Comment>().Where(comment => comment.Article.Id == articleId)
                 .OrderBy(m => m.Date).Desc.Skip(commentPage).Take(itemsCount).ListAsync());
        }
    }
}
