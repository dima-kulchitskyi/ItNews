using ItNews.Business.Entities;
using ItNews.Business.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryProvider
{
    public class CommentProvider : MemoryProvider<Comment>, ICommentProvider
    {
        private ArticleProvider articleProvider;
        private UserProvider userProvider;

        public CommentProvider()
        {
            articleProvider = new ArticleProvider();
            userProvider = new UserProvider();
        }

        public async Task<int> GetArticleCommentsCountAsync(string articleId)
        {
            return (await base.GetList()).Where(it => it.Article.Id == articleId).Count();
        }

        public async Task<IList<Comment>> GetArticleCommentsPageAsync(string articleId, int itemsCount, int commentPage)
        {
            var article = await articleProvider.Get(articleId);

            if (article == null)
                return new List<Comment>();

            return (await base.GetList())?.OrderByDescending(it => it.Date)
                .Where(it => it.Article.Id == articleId).Skip(itemsCount * commentPage).Take(itemsCount)
                .Select(it =>
                {
                    it.Article = article;
                    return it;
                }).ToList();
        }
    }
}
