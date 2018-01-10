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

        public async Task CreateArticle(Article article, string authorId)
        {
            using (var uow = unitOfWorkFactory.GetUnitOfWork())
            {
                var user = await userProvider.Get(authorId);

                article.Author = user ?? throw new InvalidOperationException("No user with given id");
                article.Date = DateTime.Now;

                uow.BeginTransaction();
                await articleProvider.SaveOrUpdate(article);
                uow.Commit();
            }
        }

        public Task<int> GetCount()
        {
            return articleProvider.GetCount();
        }

        public async Task<bool> IsOwnedBy(string articleId, string userId)
        {
            if (string.IsNullOrEmpty(articleId))
                throw new ArgumentNullException(nameof(articleId));

            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            var article = await articleProvider.Get(articleId);
            if (article == null)
                return false;

            return (article.Author?.Id == userId);
        }

        public async Task UpdateArticle(Article article, string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                throw new ArgumentNullException(nameof(authorId));


            var oldArticle = await articleProvider.Get(article.Id);
            if (oldArticle == null)
                throw new ArgumentException("Article does not exists");

            var author = await userProvider.Get(authorId);
            if (author == null)
                throw new ArgumentException($"User with {nameof(authorId)} does not exists");

            if (oldArticle.Author.Id != authorId)
                throw new InvalidOperationException($"Article is not owned by user with {nameof(authorId)}");

            article.Date = DateTime.Now;
            article.Author = author;

            using (var uow = unitOfWorkFactory.GetUnitOfWork().BeginTransaction())
            {
                await articleProvider.SaveOrUpdate(article);
                uow.Commit();
            }
        }
    }
}
