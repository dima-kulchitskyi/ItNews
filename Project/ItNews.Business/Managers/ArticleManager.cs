using ItNews.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItNews.Business.Providers;

namespace ItNews.Business.Managers
{
    public class ArticleManager : Manager<Article>
    {
        private IArticleProvider articleProvider;

        private IUserProvider userProvider;

        public ArticleManager(IArticleProvider articleProvider, IUserProvider userProvider, IUnitOfWorkFactory unitOfWorkFactory)
            : base(articleProvider, unitOfWorkFactory)
        {
            this.articleProvider = articleProvider;
            this.userProvider = userProvider;
        }

        public Task<IList<Article>> GetListSegmentAsync(int count, DateTime startDate, bool newFirst)
        {
            return articleProvider.GetListSegment(count, startDate, newFirst);
        }

        public Task<IList<Article>> GetPage(int count, int pageNumber, bool newFirst)
        {
            return articleProvider.GetPage(count, pageNumber, newFirst);
        }

        public Task<Article> GetArticle(string id)
        {
            return articleProvider.Get(id);
        }

        public async Task CreateArticle(Article article, string userId)
        {
            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                var user = await userProvider.Get(userId);

                article.Author = user ?? throw new InvalidOperationException("No user with given id");
                article.Date = DateTime.Now;

                uow.BeginTransaction();
                await articleProvider.SaveOrUpdate(article);
                uow.CommitTransaction();
            }
        }
    }
}
